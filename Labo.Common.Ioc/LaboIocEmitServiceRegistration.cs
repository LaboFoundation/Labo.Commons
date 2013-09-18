// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LaboIocEmitServiceRegistration.cs" company="Labo">
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
//   Defines the LaboIocEmitServiceRegistration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Labo.Common.Ioc.Exceptions;
    using Labo.Common.Ioc.Registration;
    using Labo.Common.Ioc.Resources;
    using Labo.Common.Reflection;
    using Labo.Common.Utils;

    /// <summary>
    /// Emit service registration.
    /// </summary>
    internal sealed class LaboIocEmitServiceRegistration : LaboIocServiceRegistration
    {
        /// <summary>
        /// The constructor binding flags.
        /// </summary>
        private const BindingFlags CONSTRUCTOR_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// The constructor invoker delegate.
        /// </summary>
        private readonly ConcurrentDictionary<ConstructorInfo, ConstructorInvoker> m_ConstructorInvokerCache;

        /// <summary>
        /// The service instance invoker.
        /// </summary>
        private readonly Lazy<Func<object>> m_ServiceInstanceInvoker;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboIocEmitServiceRegistration"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="iocContainer">The inversion of control container.</param>
        /// <param name="serviceName">The name of the service.</param>
        public LaboIocEmitServiceRegistration(Type serviceType, Type implementationType, LaboIocServiceLifetime lifetime, LaboIocContainer iocContainer, string serviceName = null)
            : base(serviceType, implementationType, lifetime, serviceName)
        {
            m_ConstructorInvokerCache = new ConcurrentDictionary<ConstructorInfo, ConstructorInvoker>();
            m_ServiceInstanceInvoker = new Lazy<Func<object>>(() => BuildServiceInvokerMethod(serviceType, iocContainer, serviceName), true);
        }

        /// <summary>
        /// Creates the service instance.
        /// </summary>
        /// <param name="containerResolver">Container resolver.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Service instance.</returns>
        public override object GetServiceInstance(IIocContainerResolver containerResolver, params object[] parameters)
        {
            if (parameters.Length > 0)
            {
                Type[] parameterTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    object parameter = parameters[i];
                    parameterTypes[i] = TypeUtils.GetType(parameter);
                }

                ConstructorInfo constructor = ImplementationType.GetConstructor(CONSTRUCTOR_BINDING_FLAGS, null, parameterTypes, null);

                if (constructor == null)
                {
                    throw new IocContainerDependencyResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.LaboIocEmitServiceCreator_CreateServiceInstance_RequiredConstructorNotMatchWithSignature, ImplementationType.FullName, StringUtils.Join(parameterTypes.Select(x => x.FullName), ", ")));
                }

                return m_ConstructorInvokerCache.GetOrAdd(constructor, c => DynamicMethodHelper.EmitConstructorInvoker(ImplementationType, c, parameterTypes))(parameters);
            }

            return m_ServiceInstanceInvoker.Value();
        }

        /// <summary>
        /// Builds the service method invoker.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="iocContainer">The inversion of control container.</param>
        /// <param name="serviceName">The name of the service.</param>
        /// <returns>Service invoker delegate.</returns>
        private static Func<object> BuildServiceInvokerMethod(Type serviceType, LaboIocContainer iocContainer, string serviceName = null)
        {
            RegistrationBuilder registrationBuilder = new RegistrationBuilder();
            return registrationBuilder.BuildServiceInvokerMethod(iocContainer, iocContainer.ModuleBuilder, serviceType, serviceName);
        }
    }
}