// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LaboIocFuncServiceCreator.cs" company="Labo">
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
//   Function service creator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;

    /// <summary>
    /// Function service creator.
    /// </summary>
    internal sealed class LaboIocFuncServiceCreator : ILaboIocServiceCreator
    {
        /// <summary>
        /// The creator function.
        /// </summary>
        private readonly Func<object> m_Creator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboIocFuncServiceCreator"/> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        public LaboIocFuncServiceCreator(Func<object> creator)
        {
            m_Creator = creator;
        }

        /// <summary>
        /// Creates the service instance.
        /// </summary>
        /// <param name="containerResolver">Container resolver.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Service instance.</returns>
        public object CreateServiceInstance(IIocContainerResolver containerResolver, params object[] parameters)
        {
            return m_Creator();
        }
    }
}