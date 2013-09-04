// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransientFactoryInstanceCreator.cs" company="Labo">
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
//   Defines the TransientFactoryInstanceCreator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Patterns
{
    using System;

    /// <summary>
    /// Transient factory instance creator class.
    /// </summary>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    internal class TransientFactoryInstanceCreator<TInstance> : IFactoryInstanceCreator<TInstance>
    {
        /// <summary>
        /// The transient instance creator
        /// </summary>
        private readonly Func<TInstance> m_Creator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientFactoryInstanceCreator{TInstance}"/> class.
        /// </summary>
        /// <param name="creator">The creator.</param>
        public TransientFactoryInstanceCreator(Func<TInstance> creator)
        {
            m_Creator = creator;
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <returns>the instance.</returns>
        public TInstance CreateInstance()
        {
            lock (m_Creator)
            {
                return m_Creator();
            }
        }
    }
}