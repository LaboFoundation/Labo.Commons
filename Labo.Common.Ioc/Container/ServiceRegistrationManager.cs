// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceRegistrationManager.cs" company="Labo">
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
//   Defines the ServiceRegistrationManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Service registration manager class.
    /// </summary>
    internal sealed class ServiceRegistrationManager : IServiceRegistrationManager
    {
        /// <summary>
        /// The service entries
        /// </summary>
        private readonly Dictionary<ServiceKey, ServiceRegistration> m_ServiceEntries;

        /// <summary>
        /// The service entries by service type
        /// </summary>
        private readonly Dictionary<Type, ServiceRegistration> m_ServiceEntriesByServiceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRegistrationManager"/> class.
        /// </summary>
        public ServiceRegistrationManager()
        {
            m_ServiceEntries = new Dictionary<ServiceKey, ServiceRegistration>();
            m_ServiceEntriesByServiceType = new Dictionary<Type, ServiceRegistration>();
        }

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        /// <param name="serviceName">Name of the service.</param>
        public void RegisterService(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime, string serviceName = null)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException("implementationType");
            }

            if (serviceName == null)
            {
                m_ServiceEntriesByServiceType[serviceType] = new ServiceRegistration(serviceType, implementationType, serviceLifetime);
            }
            else
            {
                m_ServiceEntries[new ServiceKey(serviceName, serviceType)] = new ServiceRegistration(serviceType, implementationType, serviceLifetime, serviceName);
            }
        }

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="instanceCreator">The instance creator.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        /// <param name="serviceName">Name of the service.</param>
        public void RegisterService(Type serviceType, Func<object> instanceCreator, ServiceLifetime serviceLifetime, string serviceName = null)
        {
            if (instanceCreator == null)
            {
                throw new ArgumentNullException("instanceCreator");
            }

            if (serviceName == null)
            {
                m_ServiceEntriesByServiceType[serviceType] = new ServiceRegistration(serviceType, instanceCreator, serviceLifetime);
            }
            else
            {
                m_ServiceEntries[new ServiceKey(serviceName, serviceType)] = new ServiceRegistration(serviceType, instanceCreator, serviceLifetime, serviceName);
            }
        }

        /// <summary>
        /// Gets the service registration.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>Service registration class.</returns>
        public ServiceRegistration GetServiceRegistration(Type serviceType, string serviceName = null)
        {
            ServiceRegistration serviceRegistration;

            if (serviceName == null)
            {
                m_ServiceEntriesByServiceType.TryGetValue(serviceType, out serviceRegistration);

                return serviceRegistration;
            }
            else
            {
                m_ServiceEntries.TryGetValue(new ServiceKey(serviceName, serviceType), out serviceRegistration);

                return serviceRegistration;
            }
        }

        /// <summary>
        /// Determines whether [is service registered] [the specified service type].
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>true if the service is registered otherwise false.</returns>
        public bool IsServiceRegistered(Type serviceType, string serviceName = null)
        {
            if (serviceName == null)
            {
                bool isServiceRegistered = m_ServiceEntriesByServiceType.ContainsKey(serviceType);
                if (!isServiceRegistered)
                {
                    foreach (KeyValuePair<ServiceKey, ServiceRegistration> serviceRegistration in m_ServiceEntries)
                    {
                        if (serviceRegistration.Key.ServiceType == serviceType)
                        {
                            return true;
                        }
                    }
                }

                return isServiceRegistered;
            }

            return m_ServiceEntries.ContainsKey(new ServiceKey(serviceName, serviceType));
        }

        /// <summary>
        /// Gets all service registrations.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>List of service registrations.</returns>
        public IList<ServiceRegistration> GetAllServiceRegistrations(Type serviceType)
        {
            List<ServiceRegistration> instances = new List<ServiceRegistration>();
            foreach (KeyValuePair<ServiceKey, ServiceRegistration> entry in m_ServiceEntries)
            {
                if (entry.Key.ServiceType == serviceType)
                {
                    instances.Add(entry.Value);
                }
            }

            foreach (KeyValuePair<Type, ServiceRegistration> entry in m_ServiceEntriesByServiceType)
            {
                if (entry.Key == serviceType)
                {
                    instances.Add(entry.Value);
                }
            }

            return instances;
        }
    }
}
