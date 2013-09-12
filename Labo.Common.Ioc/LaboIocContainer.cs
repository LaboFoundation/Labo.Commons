// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LaboIocContainer.cs" company="Labo">
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
//   Defines the LaboIocContainer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Labo.Common.Ioc.Exceptions;
    using Labo.Common.Ioc.Resources;
    using Labo.Common.Utils;

    /// <summary>
    /// Labo inversion of control container class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public sealed class LaboIocContainer : BaseIocContainer
    {
        /// <summary>
        /// The service entries
        /// </summary>
        private readonly Dictionary<LaboIocServiceKey, ILaboIocServiceLifetimeManager> m_ServiceEntries;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboIocContainer"/> class.
        /// </summary>
        public LaboIocContainer()
        {
            m_ServiceEntries = new Dictionary<LaboIocServiceKey, ILaboIocServiceLifetimeManager>();
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        public override void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            m_ServiceEntries[new LaboIocServiceKey(null, typeof(TImplementation))] = new LaboIocServiceSingletonLifetimeManager(new LaboIocFuncServiceCreator(() => creator(this)));
        }

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public override void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            m_ServiceEntries[new LaboIocServiceKey(name, typeof(TImplementation))] = new LaboIocServiceSingletonLifetimeManager(new LaboIocFuncServiceCreator(() => creator(this)));
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public override void RegisterSingleInstance(Type serviceType)
        {
            ValidateServiceType(serviceType, "serviceType");

            m_ServiceEntries[new LaboIocServiceKey(null, serviceType)] = new LaboIocServiceSingletonLifetimeManager(new LaboIocEmitServiceCreator(serviceType));
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public override void RegisterInstance(Type serviceType)
        {
            ValidateServiceType(serviceType, "serviceType");

            m_ServiceEntries[new LaboIocServiceKey(null, serviceType)] = new LaboIocServiceTransientLifetimeManager(new LaboIocEmitServiceCreator(serviceType));
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        public override void RegisterSingleInstance(Type serviceType, Type implementationType)
        {
            ValidateRegistrationTypes(serviceType, implementationType, "serviceType", "implementationType");

            m_ServiceEntries[new LaboIocServiceKey(null, serviceType)] = new LaboIocServiceSingletonLifetimeManager(new LaboIocEmitServiceCreator(implementationType));
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
            ValidateRegistrationTypes(serviceType, implementationType, "serviceType", "implementationType");

            m_ServiceEntries[new LaboIocServiceKey(name, serviceType)] = new LaboIocServiceSingletonLifetimeManager(new LaboIocEmitServiceCreator(implementationType));
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
            ValidateServiceType(serviceType, "serviceType");

            m_ServiceEntries[new LaboIocServiceKey(name, serviceType)] = new LaboIocServiceSingletonLifetimeManager(new LaboIocEmitServiceCreator(serviceType));
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
            m_ServiceEntries[new LaboIocServiceKey(null, typeof(TImplementation))] = new LaboIocServiceTransientLifetimeManager(new LaboIocFuncServiceCreator(() => creator(this)));
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
            ValidateRegistrationTypes(serviceType, implementationType, "serviceType", "implementationType");

            m_ServiceEntries[new LaboIocServiceKey(null, serviceType)] = new LaboIocServiceTransientLifetimeManager(new LaboIocEmitServiceCreator(implementationType));
        }

        /// <summary>
        /// Registers the instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public override void RegisterInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            m_ServiceEntries[new LaboIocServiceKey(name, typeof(TImplementation))] = new LaboIocServiceTransientLifetimeManager(new LaboIocFuncServiceCreator(() => creator(this)));
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
            ValidateRegistrationTypes(serviceType, implementationType, "serviceType", "implementationType");

            m_ServiceEntries[new LaboIocServiceKey(name, serviceType)] = new LaboIocServiceTransientLifetimeManager(new LaboIocEmitServiceCreator(implementationType));
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
            ValidateServiceType(serviceType, "serviceType");

            m_ServiceEntries[new LaboIocServiceKey(name, serviceType)] = new LaboIocServiceTransientLifetimeManager(new LaboIocEmitServiceCreator(serviceType));
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstance(Type serviceType, params object[] parameters)
        {
            ILaboIocServiceLifetimeManager serviceEntry;
            if (!m_ServiceEntries.TryGetValue(new LaboIocServiceKey(null, serviceType), out serviceEntry))
            {
                throw new InvalidOperationException();
            }

            return serviceEntry.GetServiceInstance(this, parameters);
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
            ILaboIocServiceLifetimeManager serviceEntry;
            if (!m_ServiceEntries.TryGetValue(new LaboIocServiceKey(name, serviceType), out serviceEntry))
            {
                throw new InvalidOperationException();
            }

            return serviceEntry.GetServiceInstance(this, parameters);
        }

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptional(Type serviceType, params object[] parameters)
        {
            ILaboIocServiceLifetimeManager serviceEntry;
            if (!m_ServiceEntries.TryGetValue(new LaboIocServiceKey(null, serviceType), out serviceEntry))
            {
                return null;
            }

            return serviceEntry.GetServiceInstance(this, parameters);
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
            ILaboIocServiceLifetimeManager serviceEntry;
            if (!m_ServiceEntries.TryGetValue(new LaboIocServiceKey(name, serviceType), out serviceEntry))
            {
                return null;
            }

            return serviceEntry.GetServiceInstance(this, parameters);
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>all instances.</returns>
        public override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            List<object> instances = new List<object>();
            foreach (KeyValuePair<LaboIocServiceKey, ILaboIocServiceLifetimeManager> entry in m_ServiceEntries)
            {
                if (entry.Key.ServiceType == serviceType)
                {
                    instances.Add(entry.Value.GetServiceInstance(this));
                }
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
            return m_ServiceEntries.Keys.Any(x => x.ServiceType == type);
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
            return m_ServiceEntries.ContainsKey(new LaboIocServiceKey(name, type));
        }

        /// <summary>
        /// Validates the registration types.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="serviceParamName">Name of the service param.</param>
        /// <param name="implementationParamName">Name of the implementation param.</param>
        /// <exception cref="System.ArgumentNullException">serviceType or implementationType
        /// </exception>
        /// <exception cref="Labo.Common.Ioc.Exceptions.IocContainerRegistrationException">
        /// </exception>
        private static void ValidateRegistrationTypes(Type serviceType, Type implementationType, string serviceParamName, string implementationParamName)
        {
            ValidateServiceType(serviceType, serviceParamName);

            if (implementationType == null)
            {
                throw new ArgumentNullException(implementationParamName);
            }

            if (!serviceType.IsAssignableFrom(implementationType))
            {
                throw new IocContainerRegistrationException(Strings.LaboIocContainer_ValidateRegistrationTypes_XMustBeAssignableFromY.FormatWith(serviceType.FullName, implementationType.FullName));
            }

            if (!TypeUtils.IsReferenceType(implementationType))
            {
                throw new IocContainerRegistrationException(Strings.LaboIocContainer_ValidateRegistrationTypes_ImplementationTypeMustBeReferenceType.FormatWith(implementationType.FullName));
            }

            if (implementationType.IsAbstract || implementationType.IsInterface || implementationType.IsArray || implementationType == typeof(object))
            {
                throw new IocContainerRegistrationException(Strings.LaboIocContainer_ValidateRegistrationTypes_ImplementationCannotBeInRestrictedForms.FormatWith(implementationType.FullName));
            }
        }

        /// <summary>
        /// Validates the type of the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceParamName">Name of the service param.</param>
        /// <exception cref="System.ArgumentNullException">serviceType</exception>
        /// <exception cref="Labo.Common.Ioc.Exceptions.IocContainerRegistrationException">
        /// </exception>
        private static void ValidateServiceType(Type serviceType, string serviceParamName)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(serviceParamName);
            }

            if (serviceType == typeof(Type) || serviceType == typeof(string))
            {
                throw new IocContainerRegistrationException(Strings.LaboIocContainer_ValidateServiceType_ServiceTypeCannotBeOfRestrictedTypes.FormatWith(serviceType.FullName));
            }

            if (!TypeUtils.IsReferenceType(serviceType))
            {
                throw new IocContainerRegistrationException(Strings.LaboIocContainer_ValidateServiceType_ServiceTypeMustBeReferenceType.FormatWith(serviceType.FullName));
            }
        }
    }
}
