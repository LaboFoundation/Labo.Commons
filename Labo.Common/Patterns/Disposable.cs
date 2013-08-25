using System;
using System.Collections.Generic;

namespace Labo.Common.Patterns
{
    public sealed class Disposable<TInstance> : IDisposable
        where TInstance : class, IDisposable
    {
        private Func<TInstance> m_DisposableCreator;
        private TInstance m_DisposableInstance;
        private Stack<object> m_DisposableObjectCreationStack = new Stack<object>();
        private bool m_Disposed;

        public TInstance Instance { get { return GetOrCreateInstance(); }}

        public Disposable(Func<TInstance> disposableCreator)
        {
            if (disposableCreator == null)
            {
                throw new ArgumentNullException("disposableCreator");
            }
            m_DisposableCreator = disposableCreator;
            m_DisposableObjectCreationStack.Push(new object());
        }

        ~Disposable()
        {
            Dispose(false);
        }

        private TInstance GetOrCreateInstance()
        {
            return m_DisposableInstance ?? (m_DisposableInstance = m_DisposableCreator());
        }

        public Disposable<TInstance> Using()
        {
            m_DisposableObjectCreationStack.Push(new object());
            return this;
        }

        public void Dispose()
        {
            m_DisposableObjectCreationStack.Pop();
            if (m_DisposableObjectCreationStack.Count == 0)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

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
    }
}
