// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Factory.cs" company="Labo">
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
//   Defines the Factory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Patterns
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Labo.Common.Patterns.Exception;

    /// <summary>
    /// Factory base class.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TInstance">The type of the instance.</typeparam>
    public abstract class Factory<TKey, TInstance>
    {
        /// <summary>
        /// The registration dictionary
        /// </summary>
        private static readonly IDictionary<TKey, IFactoryInstanceCreator<TInstance>> s_Dictionary = new Dictionary<TKey, IFactoryInstanceCreator<TInstance>>();

        /// <summary>
        /// Creates new TInstance using the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>new TInstance</returns>
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
        /// Throws the not found exception.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="FactoryCreateInstanceException">Key not found exception.</exception>
        protected virtual void ThrowNotFoundException(TKey key)
        {
            throw new FactoryCreateInstanceException(Convert.ToString(key, CultureInfo.CurrentCulture));
        }
    }
}
