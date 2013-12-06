// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceFactoryManager.cs" company="Labo">
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
//   Defines the ServiceFactoryManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The service factory manager class.
    /// </summary>
    internal sealed class ServiceFactoryManager : IServiceFactoryManager
    {
        /// <summary>
        /// The service registration manager
        /// </summary>
        private readonly IServiceRegistrationManager m_ServiceRegistrationManager;

        /// <summary>
        /// The service factory builder
        /// </summary>
        private readonly IServiceFactoryBuilder m_ServiceFactoryBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFactoryManager"/> class.
        /// </summary>
        /// <param name="serviceRegistrationManager">The service registration manager.</param>
        /// <param name="serviceFactoryBuilder">The service factory builder.</param>
        public ServiceFactoryManager(IServiceRegistrationManager serviceRegistrationManager, IServiceFactoryBuilder serviceFactoryBuilder)
        {
            m_ServiceRegistrationManager = serviceRegistrationManager;
            m_ServiceFactoryBuilder = serviceFactoryBuilder;
        }

        /// <summary>
        /// Gets the service factory.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>ServiceFactory class.</returns>
        public ServiceFactory GetServiceFactory(Type serviceType, string serviceName = null, params object[] parameters)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            // TODO: remove service registration manager, put service registration into servicefactory and store servicefactory in the dictionary
            ServiceRegistration serviceRegistration = m_ServiceRegistrationManager.GetServiceRegistration(serviceType, serviceName);

            return GetOrBuildServiceFactory(serviceRegistration);
        }

        /// <summary>
        /// Gets all service factories.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The list of service factories.</returns>
        public IList<ServiceFactory> GetAllServiceFactories(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            IList<ServiceRegistration> serviceRegistrations = m_ServiceRegistrationManager.GetAllServiceRegistrations(serviceType);
            IList<ServiceFactory> serviceFactories = new List<ServiceFactory>(serviceRegistrations.Count);
            for (int i = 0; i < serviceRegistrations.Count; i++)
            {
                ServiceRegistration serviceRegistration = serviceRegistrations[i];
                serviceFactories.Add(GetOrBuildServiceFactory(serviceRegistration));
            }

            return serviceFactories;
        }

        /// <summary>
        /// Gets the service factory or build service factory.
        /// </summary>
        /// <param name="serviceRegistration">The service registration.</param>
        /// <returns>The service factory.</returns>
        private ServiceFactory GetOrBuildServiceFactory(ServiceRegistration serviceRegistration)
        {
            if (serviceRegistration.ServiceFactory == null)
            {
                return m_ServiceFactoryBuilder.BuildServiceFactory(serviceRegistration);
            }

            return serviceRegistration.ServiceFactory;
        }
    }
}
