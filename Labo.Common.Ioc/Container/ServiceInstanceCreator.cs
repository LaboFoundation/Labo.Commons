﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceInstanceCreator.cs" company="Labo">
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
//   The service instance creator class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The service instance creator class.
    /// </summary>
    internal sealed class ServiceInstanceCreator
    {
        /// <summary>
        /// The service factory builder
        /// </summary>
        private readonly IServiceFactoryBuilder m_ServiceFactoryBuilder;
        
        /// <summary>
        /// The service registration
        /// </summary>
        private readonly ServiceRegistration m_ServiceRegistration;
        
        /// <summary>
        /// The service factory
        /// </summary>
        private ServiceFactory m_ServiceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInstanceCreator"/> class.
        /// </summary>
        /// <param name="serviceFactoryBuilder">The service factory builder.</param>
        /// <param name="serviceRegistration">The service registration.</param>
        public ServiceInstanceCreator(IServiceFactoryBuilder serviceFactoryBuilder, ServiceRegistration serviceRegistration)
        {
            m_ServiceFactoryBuilder = serviceFactoryBuilder;
            m_ServiceRegistration = serviceRegistration;
        }

        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The service instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetServiceInstance(object[] parameters)
        {
            return GetServiceFactory().GetServiceInstance(parameters);
        }

        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <returns>The service instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetServiceInstance()
        {
            return GetServiceFactory().GetServiceInstance();
        }

        /// <summary>
        /// Gets the service factory.
        /// </summary>
        /// <returns>The service factory.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ServiceFactory GetServiceFactory()
        {
            return m_ServiceFactory ?? (m_ServiceFactory = m_ServiceFactoryBuilder.BuildServiceFactory(m_ServiceRegistration));
        }
    }
}