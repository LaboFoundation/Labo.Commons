// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicMethodCache.cs" company="Labo">
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
//   Defines the DynamicMethodCache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Reflection
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;

    /// <summary>
    /// Dynamic method cache class.
    /// </summary>
    internal sealed class DynamicMethodCache
    {
        /// <summary>
        /// The the delegate entries dictionary.
        /// </summary>
        private readonly ConcurrentDictionary<MemberInfo, object> m_Entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicMethodCache"/> class.
        /// </summary>
        public DynamicMethodCache()
        {
            m_Entries = new ConcurrentDictionary<MemberInfo, object>();
        }

        /// <summary>
        /// Gets or adds the method delegate.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
        /// <param name="memberInfo">The member information.</param>
        /// <param name="creatorFunc">The creator function.</param>
        /// <param name="cacheStrategy">The cache strategy.</param>
        /// <returns>The method delegate.</returns>
        public TDelegate GetOrAddDelegate<TDelegate>(MemberInfo memberInfo, Func<TDelegate> creatorFunc, DynamicMethodCacheStrategy cacheStrategy)
        {
            object entry = m_Entries.AddOrUpdate(
                                                memberInfo,
                                                x => CreateDelegate(creatorFunc, cacheStrategy),
                                                (x, y) =>
                                                    {
                                                        WeakReference weakReference = y as WeakReference;
                                                        if (weakReference != null && weakReference.IsAlive)
                                                        {
                                                            return weakReference.Target;
                                                        }

                                                        return CreateDelegate(creatorFunc, cacheStrategy);
                                                    });
            return (TDelegate)(entry is WeakReference ? ((WeakReference)entry).Target : entry);
        }

        /// <summary>
        /// Creates the delegate.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
        /// <param name="creatorFunc">The creator function.</param>
        /// <param name="cacheStrategy">The cache strategy.</param>
        /// <returns>The delegate value</returns>
        private static object CreateDelegate<TDelegate>(Func<TDelegate> creatorFunc, DynamicMethodCacheStrategy cacheStrategy)
        {
            if (cacheStrategy == DynamicMethodCacheStrategy.Temporary)
            {
                return new WeakReference(creatorFunc());
            }

            return creatorFunc();
        }
    }
}
