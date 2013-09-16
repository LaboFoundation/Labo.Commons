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
        private sealed class ServiceInstanceInvoker
        {
            private readonly Func<Func<object>[], object> m_InvocationFunction;

            /// <summary>
            /// The parameters
            /// </summary>
            private readonly Func<object>[] m_Parameters;

            public ServiceInstanceInvoker(Func<Func<object>[], object> invocationFunction, Func<object>[] parameters)
            {
                m_InvocationFunction = invocationFunction;
                m_Parameters = parameters;
            }

            public object InvokeService()
            {
                return m_InvocationFunction(m_Parameters);
            }
        }

        /// <summary>
        /// The constructor binding flags.
        /// </summary>
        private const BindingFlags CONSTRUCTOR_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// The constructor invoker delegate.
        /// </summary>
        private readonly ConcurrentDictionary<ConstructorInfo, ConstructorInvoker> m_ConstructorInvokerCache;

        /// <summary>
        /// The service implementation type
        /// </summary>
        private readonly Type m_ServiceImplementationType;

        /// <summary>
        /// The service instance invoker.
        /// </summary>
        private readonly Lazy<ServiceInstanceInvoker> m_ServiceInstanceInvoker;

        /// <summary>
        /// Gets the type of the service implementation.
        /// </summary>
        /// <value>
        /// The type of the service implementation.
        /// </value>
        public Type ServiceImplementationType
        {
            get { return m_ServiceImplementationType; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboIocEmitServiceCreator"/> class.
        /// </summary>
        /// <param name="serviceImplemetationType">Type of the service implementation.</param>
        /// <param name="lifetimeManagerProvider">Service lifetime manager provider.</param>
        public LaboIocEmitServiceCreator(Type serviceImplemetationType, ILaboIocLifetimeManagerProvider lifetimeManagerProvider)
        {
            m_ServiceImplementationType = serviceImplemetationType;
            m_ConstructorInvokerCache = new ConcurrentDictionary<ConstructorInfo, ConstructorInvoker>();
            m_ServiceInstanceInvoker = new Lazy<ServiceInstanceInvoker>(() => CreateConstructorInvocationDelegate(serviceImplemetationType, lifetimeManagerProvider), true);
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

            return InvokeServiceInstance();
        }

        /// <summary>
        /// Generates the service instance creator.
        /// </summary>
        /// <returns>Service instance creator delegate.</returns>
        public Func<object> GenerateServiceInstanceCreator()
        {
            return InvokeServiceInstance;
        }

        /// <summary>
        /// Gets the constructor info of the service implementation type.
        /// </summary>
        /// <param name="serviceImplementationType">Type of the service implementation.</param>
        /// <returns>The constructor info.</returns>
        /// <exception cref="IocContainerDependencyResolutionException">Thrown when no suited constructor is found.</exception>
        private static ConstructorInfo GetConstructorInfo(Type serviceImplementationType)
        {
            ConstructorInfo constructor = serviceImplementationType.GetConstructors(CONSTRUCTOR_BINDING_FLAGS).FirstOrDefault();

            if (constructor == null)
            {
                throw new IocContainerDependencyResolutionException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.LaboIocEmitServiceCreator_CreateServiceInstance_NoConstructorsCanBeFound,
                        serviceImplementationType.FullName));
            }

            return constructor;
        }

        /// <summary>
        /// Creates the constructor invocation delegate.
        /// </summary>
        /// <param name="serviceImplementationType">
        /// Type of the service implementation.
        /// </param>
        /// <param name="lifetimeManagerProvider">
        /// The lifetime Manager Provider.
        /// </param>
        /// <returns>
        /// The constructor invocation delegate.
        /// </returns>
        private static ServiceInstanceInvoker CreateConstructorInvocationDelegate(Type serviceImplementationType, ILaboIocLifetimeManagerProvider lifetimeManagerProvider)
        {
            LaboIocServiceCreationInfo serviceCreationInfo = new LaboIocServiceCreationInfo(GetConstructorInfo(serviceImplementationType), lifetimeManagerProvider);
            ConstructorInfo constructor = serviceCreationInfo.ServiceConstructor;

            DynamicMethod createMethod = new DynamicMethod("CreateServiceInstance", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, typeof(object), new[] { typeof(Func<object>).MakeArrayType() }, serviceImplementationType, true);

            ILGenerator generator = createMethod.GetILGenerator();

            MethodInfo createDependentServiceMethod = typeof(Func<object>).GetMethod("Invoke");

            Type[] dependentServiceTypes = serviceCreationInfo.DependentServiceTypes;
            int constructorParametersLength = dependentServiceTypes.Length;
            for (int i = 0; i < constructorParametersLength; i++)
            {
                Type dependentServiceType = dependentServiceTypes[i];

                EmitHelper.Ldarg0(generator);
                EmitHelper.LdcI4(generator, i);
                EmitHelper.LdelemRef(generator);
                EmitHelper.CallVirt(generator, createDependentServiceMethod);
                EmitHelper.Castclass(generator, dependentServiceType);
            }

            EmitHelper.Newobj(generator, constructor);
            EmitHelper.Ret(generator);

            Func<Func<object>[], object> constructorInvocationDelegate = (Func<Func<object>[], object>)createMethod.CreateDelegate(typeof(Func<Func<object>[], object>));

            return new ServiceInstanceInvoker(constructorInvocationDelegate, serviceCreationInfo.DependentServiceCreators);
        }

        /// <summary>
        /// Invokes the service instance.
        /// </summary>
        /// <returns>The service instance.</returns>
        private object InvokeServiceInstance()
        {
            return m_ServiceInstanceInvoker.Value.InvokeService();
        }
    }
}