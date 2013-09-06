// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceProducerContainerFactory.cs" company="Labo">
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
//   Defines the InstanceProducerContainerFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.SimpleInjector
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using global::SimpleInjector;

    /// <summary>
    /// Instance producer container factory class.
    /// </summary>
    internal sealed class InstanceProducerContainerFactory : SortedList<string, InstanceProducer>
    {
        /// <summary>
        /// The container
        /// </summary>
        private readonly Container m_Container;

        /// <summary>
        /// The service type name cache
        /// </summary>
        private readonly SortedList<Type, SortedSet<string>> m_ServiceTypeNameCache; 

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProducerContainerFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public InstanceProducerContainerFactory(Container container)
        {
            m_Container = container;
            m_ServiceTypeNameCache = new SortedList<Type, SortedSet<string>>();
        }

        /// <summary>
        /// Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="name">The name.</param>
        /// <param name="lifestyle">The lifestyle.</param>
        public void Register(Type serviceType, Type implementationType, string name, Lifestyle lifestyle = null)
        {
            lifestyle = lifestyle ?? Lifestyle.Transient;

            Registration registration = lifestyle.CreateRegistration(serviceType, implementationType, m_Container);

            InstanceProducer producer = new InstanceProducer(serviceType, registration);

            AddServiceInstanceProducer(serviceType, name, producer);
        }

        /// <summary>
        /// Registers the specified instance creator.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instanceCreator">The instance creator.</param>
        /// <param name="name">The name.</param>
        /// <param name="lifestyle">The lifestyle.</param>
        public void Register<TService>(Func<TService> instanceCreator, string name, Lifestyle lifestyle = null) 
            where TService : class
        {
            lifestyle = lifestyle ?? Lifestyle.Transient;

            Registration registration = lifestyle.CreateRegistration(instanceCreator, m_Container);

            Type serviceType = typeof(TService);
            InstanceProducer producer = new InstanceProducer(serviceType, registration);

            AddServiceInstanceProducer(serviceType, name, producer);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        public object GetInstance(Type serviceType, string name)
        {
            InstanceProducer instanceProducer = this[GetServiceTypeKey(serviceType, name)];
            Type type = instanceProducer.ServiceType;
            return instanceProducer.GetInstance();
        }

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance or null.</returns>
        public object GetInstanceOptional(Type serviceType, string name)
        {
            InstanceProducer instanceProducer = this[GetServiceTypeKey(serviceType, name)];
            if (instanceProducer == null)
            {
                return null;
            }

            Type type = instanceProducer.ServiceType;
            return instanceProducer.GetInstance();
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>all instances.</returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            SortedSet<string> serviceTypeNames;
            if (m_ServiceTypeNameCache.TryGetValue(serviceType, out serviceTypeNames))
            {
                List<object> instances = new List<object>(serviceTypeNames.Count);
                foreach (string serviceTypeName in serviceTypeNames)
                {
                    instances.Add(this.GetInstance(serviceType,  serviceTypeName));
                }

                return instances;
            }

            return Enumerable.Empty<object>();
        }

        /// <summary>
        /// Gets the service type key.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>Service type key.</returns>
        private static string GetServiceTypeKey(Type serviceType, string name)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}+{1}", serviceType.AssemblyQualifiedName, name);
        }

        /// <summary>
        /// Adds the service instance producer.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <param name="producer">The producer.</param>
        private void AddServiceInstanceProducer(Type serviceType, string name, InstanceProducer producer)
        {
            SortedSet<string> serviceTypeNames;
            string serviceTypeKey = GetServiceTypeKey(serviceType, name);
            if (m_ServiceTypeNameCache.TryGetValue(serviceType, out serviceTypeNames))
            {
                if (!serviceTypeNames.Contains(serviceTypeKey))
                {
                    serviceTypeNames.Add(name);
                }
            }
            else
            {
                m_ServiceTypeNameCache.Add(serviceType, new SortedSet<string> { name });
            }

            Add(serviceTypeKey, producer);
        }
    }
}