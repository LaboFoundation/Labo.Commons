// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LaboIocEmitServiceCreator.cs" company="Labo">
//   The MIT License (MIT)
//   
//   Copyright (c) 2013 Bora Akgun
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to
//   use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//   the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Reflection emit service creator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Ioc.Exceptions;
    using Labo.Common.Ioc.Resources;
    using Labo.Common.Reflection;
    using Labo.Common.Utils;

    /// <summary>
    /// Reflection emit service creator.
    /// </summary>
    internal sealed class LaboIocEmitServiceCreator : ILaboIocServiceCreator
    {
        /// <summary>
        /// Service invoker delegate.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <returns>service instance.</returns>
        private delegate object ServiceInvoker(IIocContainerResolver resolver);

        /// <summary>
        /// The constructor binding flags.
        /// </summary>
        private const BindingFlags CONSTRUCTOR_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// The constructor invoker delegate.
        /// </summary>
        private readonly ConcurrentDictionary<ConstructorInfo, ConstructorInvoker> m_ConstructorInvokerCache;

        /// <summary>
        /// The default constructor invoker cache.
        /// </summary>
        private readonly Lazy<ServiceInvoker> m_DefaultConstructorInvoker; 

        /// <summary>
        /// The circular dependency validator.
        /// </summary>
        private readonly CircularDependencyValidator m_CircularDependencyValidator;

        /// <summary>
        /// The service implementation type
        /// </summary>
        private readonly Type m_ServiceImplementationType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboIocEmitServiceCreator"/> class.
        /// </summary>
        /// <param name="serviceImplemetationType">Type of the service implementation.</param>
        public LaboIocEmitServiceCreator(Type serviceImplemetationType)
        {
            m_ServiceImplementationType = serviceImplemetationType;
            m_CircularDependencyValidator = new CircularDependencyValidator(serviceImplemetationType);
            m_ConstructorInvokerCache = new ConcurrentDictionary<ConstructorInfo, ConstructorInvoker>();

            m_DefaultConstructorInvoker = new Lazy<ServiceInvoker>(() => CreateConstructorInvocationInfo(serviceImplemetationType), true);
        }

        /// <summary>
        /// Resolver the instance value using container resolver. If null returns from container resolver than returns the default value of the type.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="containerResolver">The container resolver.</param>
        /// <returns>The instance.</returns>
        public static TType GetValueOfType<TType>(IIocContainerResolver containerResolver)
        {
            object instance = containerResolver.GetInstanceOptional(typeof(TType));
            return instance == null ? (TType)TypeUtils.GetDefaultValueOfType(typeof(TType)) : (TType)instance;
        }

        /// <summary>
        /// Creates the service instance.
        /// </summary>
        /// <param name="containerResolver">Container resolver.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Service instance.</returns>
        public object CreateServiceInstance(IIocContainerResolver containerResolver, params object[] parameters)
        {
            if (parameters.Length > 0)
            {
                Type[] parameterTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    object parameter = parameters[i];
                    parameterTypes[i] = TypeUtils.GetType(parameter);
                }

                ConstructorInfo constructor = m_ServiceImplementationType.GetConstructor(CONSTRUCTOR_BINDING_FLAGS, null, parameterTypes, null);

                if (constructor == null)
                {
                    throw new IocContainerDependencyResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.LaboIocEmitServiceCreator_CreateServiceInstance_RequiredConstructorNotMatchWithSignature, m_ServiceImplementationType.FullName, StringUtils.Join(parameterTypes.Select(x => x.FullName), ", ")));
                }

                return m_ConstructorInvokerCache.GetOrAdd(constructor, c => DynamicMethodHelper.EmitConstructorInvoker(m_ServiceImplementationType, c, parameterTypes))(parameters);
            }

            try
            {
                m_CircularDependencyValidator.CheckCircularDependency();

                object serviceInstance = m_DefaultConstructorInvoker.Value(containerResolver);

                m_CircularDependencyValidator.Disable();

                return serviceInstance;
            }
            catch 
            {
                m_CircularDependencyValidator.Release();

                throw;
            }
        }

        /// <summary>
        /// Creates constructor invocation info.
        /// </summary>
        /// <param name="serviceImplementationType">Type of the service implementation.</param>
        /// <returns>Constructor invocation info.</returns>
        /// <exception cref="IocContainerDependencyResolutionException">Thrown when no constructor found in <param name="serviceImplementationType">serviceImplementationType</param></exception>
        private static ServiceInvoker CreateConstructorInvocationInfo(Type serviceImplementationType)
        {
            ConstructorInfo constructor = serviceImplementationType.GetConstructors(CONSTRUCTOR_BINDING_FLAGS).FirstOrDefault();

            if (constructor == null)
            {
                throw new IocContainerDependencyResolutionException(string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.LaboIocEmitServiceCreator_CreateServiceInstance_NoConstructorsCanBeFound,
                        serviceImplementationType.FullName));
            }

            DynamicMethod createMethod = new DynamicMethod("CreateServiceInstance", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, typeof(object), new[] { typeof(IIocContainerResolver) }, serviceImplementationType, true);

            ParameterInfo[] constructorParameters = constructor.GetParameters();

            ILGenerator generator = createMethod.GetILGenerator();

            // TODO: generate service creation codes instead of recursive call
            MethodInfo getValueOfTypeMethod = typeof(LaboIocEmitServiceCreator).GetMethod("GetValueOfType", BindingFlags.Public | BindingFlags.Static);

            int constructorParametersLength = constructorParameters.Length;
            for (int i = 0; i < constructorParametersLength; i++)
            {
                ParameterInfo constructorParameter = constructorParameters[i];
                Type constructorParameterType = constructorParameter.ParameterType;

                EmitHelper.Ldarg0(generator);
                EmitHelper.Call(generator, getValueOfTypeMethod.MakeGenericMethod(constructorParameterType));
            }

            EmitHelper.Newobj(generator, constructor);
            EmitHelper.Ret(generator);

            return (ServiceInvoker)createMethod.CreateDelegate(typeof(ServiceInvoker));
        }
    }
}