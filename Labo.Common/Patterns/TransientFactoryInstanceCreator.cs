using System;

namespace Labo.Common.Patterns
{
    internal class TransientFactoryInstanceCreator<TInstance> : IFactoryInstanceCreator<TInstance>
    {
        private readonly Func<TInstance> m_Creator;

        public TransientFactoryInstanceCreator(Func<TInstance> creator)
        {
            m_Creator = creator;
        }

        public TInstance CreateInstance()
        {
            lock (m_Creator)
            {
                return m_Creator();
            }
        }
    }
}