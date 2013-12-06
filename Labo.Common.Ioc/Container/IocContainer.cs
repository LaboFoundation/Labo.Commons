// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IocContainer.cs" company="Labo">
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
//   Defines the IocContainer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The labo ioc container implementation.
    /// </summary>
    public sealed class IocContainer : BaseIocContainer
    {
        /// <summary>
        /// The service registration manager
        /// </summary>
        private IServiceRegistrationManager m_ServiceRegistrationManager;

        /// <summary>
        /// The service factory manager
        /// </summary>
        private IServiceFactoryManager m_ServiceFactoryManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IocContainer"/> class.
        /// </summary>
        public IocContainer()
        {
            ServiceRegistrationManager serviceRegistrationManager = new ServiceRegistrationManager();

            Init(serviceRegistrationManager, new ServiceFactoryManager(serviceRegistrationManager, new ServiceFactoryBuilder(new DynamicAssemblyBuilder(new DynamicAssemblyManager()), new ServiceConstructorChooser(), serviceRegistrationManager)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IocContainer"/> class.
        /// </summary>
        /// <param name="serviceRegistrationManager">The service registration manager.</param>
        /// <param name="serviceFactoryManager">The service factory manager.</param>
        internal IocContainer(IServiceRegistrationManager serviceRegistrationManager, IServiceFactoryManager serviceFactoryManager)
        {
            Init(serviceRegistrationManager, serviceFactoryManager);
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        public override void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            m_ServiceRegistrationManager.RegisterService(typeof(TImplementation), () => creator(this), ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public override void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            m_ServiceRegistrationManager.RegisterService(typeof(TImplementation), () => creator(this), ServiceLifetime.Singleton, name);
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public override void RegisterSingleInstance(Type serviceType)
        {
            m_ServiceRegistrationManager.RegisterService(serviceType, serviceType, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public override void RegisterInstance(Type serviceType)
        {
            m_ServiceRegistrationManager.RegisterService(serviceType, serviceType, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        public override void RegisterSingleInstance(Type serviceType, Type implementationType)
        {
            m_ServiceRegistrationManager.RegisterService(serviceType, implementationType, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// The register singleton named instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="implementationType">
        /// The implementation type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public override void RegisterSingleInstanceNamed(Type serviceType, Type implementationType, string name)
        {
            m_ServiceRegistrationManager.RegisterService(serviceType, implementationType, ServiceLifetime.Singleton, name);
        }

        /// <summary>
        /// The register singleton named instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public override void RegisterSingleInstanceNamed(Type serviceType, string name)
        {
            m_ServiceRegistrationManager.RegisterService(serviceType, serviceType, ServiceLifetime.Singleton, name);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="creator">
        /// The creator.
        /// </param>
        /// <typeparam name="TImplementation">
        /// The implementation type.
        /// </typeparam>
        public override void RegisterInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            m_ServiceRegistrationManager.RegisterService(typeof(TImplementation), () => creator(this), ServiceLifetime.Transient);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="implementationType">
        /// The implementation type.
        /// </param>
        public override void RegisterInstance(Type serviceType, Type implementationType)
        {
            m_ServiceRegistrationManager.RegisterService(serviceType, implementationType, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Registers the instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public override void RegisterInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            m_ServiceRegistrationManager.RegisterService(typeof(TImplementation), () => creator(this), ServiceLifetime.Transient, name);
        }

        /// <summary>
        /// The register named instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="implementationType">
        /// The implementation type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public override void RegisterInstanceNamed(Type serviceType, Type implementationType, string name)
        {
            m_ServiceRegistrationManager.RegisterService(serviceType, implementationType, ServiceLifetime.Transient, name);
        }

        /// <summary>
        /// The register singleton named instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public override void RegisterInstanceNamed(Type serviceType, string name)
        {
            m_ServiceRegistrationManager.RegisterService(serviceType, serviceType, ServiceLifetime.Transient, name);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstance(Type serviceType, params object[] parameters)
        {
            return m_ServiceFactoryManager.GetServiceFactory(serviceType).GetServiceInstance(parameters);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceByName(Type serviceType, string name, params object[] parameters)
        {
            return m_ServiceFactoryManager.GetServiceFactory(serviceType, name).GetServiceInstance(parameters);
        }

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptional(Type serviceType, params object[] parameters)
        {
            if (IsRegistered(serviceType))
            {
                return GetInstance(serviceType, parameters);
            }

            return null;
        }

        /// <summary>
        /// Gets the instance optional by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptionalByName(Type serviceType, string name, params object[] parameters)
        {
            if (IsRegistered(serviceType, name))
            {
                return GetInstanceByName(serviceType, name, parameters);
            }

            return null;
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>all instances.</returns>
        public override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            IList<ServiceFactory> serviceFactories = m_ServiceFactoryManager.GetAllServiceFactories(serviceType);
            List<object> instances = new List<object>(serviceFactories.Count);

            for (int i = 0; i < serviceFactories.Count; i++)
            {
                ServiceFactory serviceFactory = serviceFactories[i];
                instances.Add(serviceFactory.GetServiceInstance());
            }

            return instances;
        }

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRegistered(Type type)
        {
            return m_ServiceRegistrationManager.IsServiceRegistered(type);
        }

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRegistered(Type type, string name)
        {
            return m_ServiceRegistrationManager.IsServiceRegistered(type, name);
        }

        /// <summary>
        /// Initializes the specified service registration manager.
        /// </summary>
        /// <param name="serviceRegistrationManager">The service registration manager.</param>
        /// <param name="serviceFactoryManager">The service factory manager.</param>
        private void Init(IServiceRegistrationManager serviceRegistrationManager, IServiceFactoryManager serviceFactoryManager)
        {
            m_ServiceRegistrationManager = serviceRegistrationManager;
            m_ServiceFactoryManager = serviceFactoryManager;
        }
    }
}
