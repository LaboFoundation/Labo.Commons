// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LaboIocServiceRegistration.cs" company="Labo">
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
//   Defines the LaboIocServiceRegistration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;

    /// <summary>
    /// Service registration class.
    /// </summary>
    public abstract class LaboIocServiceRegistration
    {
        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets the type of the implementation.
        /// </summary>
        /// <value>
        /// The type of the implementation.
        /// </value>
        public Type ImplementationType { get; private set; }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; private set; }

        /// <summary>
        /// Gets the instance creator.
        /// </summary>
        /// <value>
        /// The instance creator.
        /// </value>
        public Func<object> InstanceCreator { get; private set; }

        /// <summary>
        /// Gets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public LaboIocServiceLifetime Lifetime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboIocServiceRegistration"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="serviceName">The name of the service.</param>
        protected LaboIocServiceRegistration(Type serviceType, Type implementationType, LaboIocServiceLifetime lifetime, string serviceName = null)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            Lifetime = lifetime;
            ServiceName = serviceName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboIocServiceRegistration"/> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="serviceName">The name of the service.</param>
        protected LaboIocServiceRegistration(Func<object> creator, LaboIocServiceLifetime lifetime, string serviceName = null)
        {
            InstanceCreator = creator;
            Lifetime = lifetime;
            ServiceName = serviceName;
        }

        /// <summary>
        /// Creates the service instance.
        /// </summary>
        /// <param name="containerResolver">Container resolver.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Service instance.</returns>
        public abstract object GetServiceInstance(IIocContainerResolver containerResolver, params object[] parameters);
    }
}
