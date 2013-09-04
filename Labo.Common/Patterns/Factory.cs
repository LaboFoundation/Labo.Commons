using System;
using System.Collections.Generic;
using System.Globalization;

using Labo.Common.Patterns.Exception;

namespace Labo.Common.Patterns
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TInstance"></typeparam>
    public abstract class Factory<TKey, TInstance>
    {
        private static readonly IDictionary<TKey, IFactoryInstanceCreator<TInstance>> s_Dictionary = new Dictionary<TKey, IFactoryInstanceCreator<TInstance>>();

        /// <summary>
        /// Registers the provider.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="creator">The creator.</param>
        protected static void RegisterProvider(TKey key, Func<TInstance> creator)
        {
            RegisterProvider(key, creator, false);
        }

        /// <summary>
        /// Registers the provider.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="transient">if set to <c>true</c> [transient].</param>
        protected static void RegisterProvider(TKey key, Func<TInstance> creator, bool transient)
        {
            if (transient)
            {
                s_Dictionary.Add(key, new TransientFactoryInstanceCreator<TInstance>(creator));
            }
            else
            {
                s_Dictionary.Add(key, new LazyFactoryInstanceCreator<TInstance>(creator, true));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TInstance Create(TKey key)
        {
            IFactoryInstanceCreator<TInstance> provider;
            if (!s_Dictionary.TryGetValue(key, out provider))
            {
                ThrowNotFoundException(key);
            }
            return provider.CreateInstance();
        }

        /// <summary>
        /// Throws the not found exception.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="FactoryCreateInstanceException"></exception>
        protected virtual void ThrowNotFoundException(TKey key)
        {
            throw new FactoryCreateInstanceException(Convert.ToString(key, CultureInfo.CurrentCulture));
        }
    }
}
