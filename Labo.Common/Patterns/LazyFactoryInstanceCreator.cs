using System;

namespace Labo.Common.Patterns
{
    internal class LazyFactoryInstanceCreator<TInstance> : IFactoryInstanceCreator<TInstance>
    {
        private readonly Lazy<TInstance> m_Lazy; 

        public LazyFactoryInstanceCreator(Func<TInstance> creator, bool threadSafe)
        {
            m_Lazy = new Lazy<TInstance>(creator, threadSafe);
        }

        public TInstance CreateInstance()
        {
            return m_Lazy.Value;
        }
    }
}