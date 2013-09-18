// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LaboIocFuncServiceRegistration.cs" company="Labo">
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
//   Function service registration implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;

    /// <summary>
    /// Function service registration implementation.
    /// </summary>
    internal sealed class LaboIocFuncServiceRegistration : LaboIocServiceRegistration
    {
        /// <summary>
        /// The lazy instance creator
        /// </summary>
        private readonly Lazy<object> m_LazyCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboIocFuncServiceRegistration"/> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="serviceName">The name of the service.</param>
        public LaboIocFuncServiceRegistration(Func<object> creator, LaboIocServiceLifetime lifetime, string serviceName = null)
            : base(creator, lifetime, serviceName)
        {
            if (lifetime == LaboIocServiceLifetime.Singleton)
            {
                m_LazyCreator = new Lazy<object>(creator, true);
            }
        }

        /// <summary>
        /// Creates the service instance.
        /// </summary>
        /// <param name="containerResolver">Container resolver.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Service instance.</returns>
        public override object GetServiceInstance(IIocContainerResolver containerResolver, params object[] parameters)
        {
            if (Lifetime == LaboIocServiceLifetime.Singleton)
            {
                return m_LazyCreator.Value;
            }

            return InstanceCreator();
        }
    }
}