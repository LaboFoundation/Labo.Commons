// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Disposable.cs" company="Labo">
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
//   Defines the Disposable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Patterns
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Disposable object class.
    /// </summary>
    /// <typeparam name="TInstance">Disposable instance.</typeparam>
    public sealed class Disposable<TInstance> : IDisposable
        where TInstance : class, IDisposable
    {
        /// <summary>
        /// The disposable instance creator.
        /// </summary>
        private Func<TInstance> m_DisposableCreator;

        /// <summary>
        /// The disposable object instance.
        /// </summary>
        private TInstance m_DisposableInstance;

        /// <summary>
        /// The disposable object creation stack.
        /// </summary>
        private Stack<object> m_DisposableObjectCreationStack = new Stack<object>();

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool m_Disposed;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public TInstance Instance
        {
            get
            {
                return GetOrCreateInstance();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Disposable{TInstance}"/> class.
        /// </summary>
        /// <param name="disposableCreator">The disposable creator.</param>
        /// <exception cref="System.ArgumentNullException">disposableCreator</exception>
        public Disposable(Func<TInstance> disposableCreator)
        {
            if (disposableCreator == null)
            {
                throw new ArgumentNullException("disposableCreator");
            }

            m_DisposableCreator = disposableCreator;
            m_DisposableObjectCreationStack.Push(new object());
        }

        /// <summary>
        /// Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1643:DestructorSummaryDocumentationMustBeginWithStandardText", Justification = "Reviewed. Suppression is OK here.")]
        ~Disposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// Usings this instance.
        /// </summary>
        /// <returns>Disposable object.</returns>
        public Disposable<TInstance> Using()
        {
            m_DisposableObjectCreationStack.Push(new object());
            return this;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            m_DisposableObjectCreationStack.Pop();
            if (m_DisposableObjectCreationStack.Count == 0)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                if (m_DisposableInstance != null)
                {
                    m_DisposableInstance.Dispose();
                    m_DisposableInstance = null;
                }

                m_DisposableObjectCreationStack = null;
                m_DisposableCreator = null;
                m_Disposed = true;
            }
        }

        /// <summary>
        /// Gets the or create instance.
        /// </summary>
        /// <returns>Disposable object instance.</returns>
        private TInstance GetOrCreateInstance()
        {
            return m_DisposableInstance ?? (m_DisposableInstance = m_DisposableCreator());
        }
    }
}
