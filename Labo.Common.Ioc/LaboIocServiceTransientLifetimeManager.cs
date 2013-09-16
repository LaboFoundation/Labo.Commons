// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LaboIocServiceTransientLifetimeManager.cs" company="Labo">
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
//   Transient lifetime manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;

    /// <summary>
    /// Transient lifetime manager.
    /// </summary>
    internal sealed class LaboIocServiceTransientLifetimeManager : ILaboIocServiceLifetimeManager
    {
        /// <summary>
        /// The service creator
        /// </summary>
        private readonly ILaboIocServiceCreator m_IocServiceCreator;

        /// <summary>
        /// Gets the service creator.
        /// </summary>
        /// <value>
        /// The service creator.
        /// </value>
        public ILaboIocServiceCreator ServiceCreator
        {
            get { return m_IocServiceCreator; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboIocServiceTransientLifetimeManager"/> class.
        /// </summary>
        /// <param name="iocServiceCreator">The inversion of control service creator.</param>
        public LaboIocServiceTransientLifetimeManager(ILaboIocServiceCreator iocServiceCreator)
        {
            m_IocServiceCreator = iocServiceCreator;
        }

        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <param name="resolver">Container resolver</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>service instance.</returns>
        public object GetServiceInstance(IIocContainerResolver resolver, params object[] parameters)
        {
            return m_IocServiceCreator.CreateServiceInstance(resolver, parameters);
        }

        /// <summary>
        /// Gets the service instance creator.
        /// </summary>
        /// <returns>service instance creator delegate.</returns>
        public Func<object> GetServiceInstanceCreator()
        {
            return m_IocServiceCreator.GenerateServiceInstanceCreator();
        }

        public LaboIocServiceLifetime Lifetime
        {
            get { return LaboIocServiceLifetime.Transient; }
        }
    }
}