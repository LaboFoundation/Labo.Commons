// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="Labo">
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
//   Defines the CollectionExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace System
{
    using System.Collections.Generic;

    using Labo.Common.Comparer;

    /// <summary>
    /// Collection extensions.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Performs the specified action for each element of the System.Collections.Generic.IEnumerable.
        /// </summary>
        /// <typeparam name="TItem">The type of the collection item.</typeparam>
        /// <param name="collection">The target collection.</param>
        /// <param name="action">The action to be performed foreach element of the collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// collection
        /// or
        /// action
        /// </exception>
        public static void ForEach<TItem>(this IEnumerable<TItem> collection, Action<TItem> action)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in collection)
            {
                action(item);
            }
        }

        /// <summary>
        /// Reverses the specified comparer.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="comparer">The comparer.</param>
        /// <returns>Reversed comparer.</returns>
        /// <exception cref="System.ArgumentNullException">comparer</exception>
        public static IComparer<TItem> Reverse<TItem>(this IComparer<TItem> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            return new ComparerReverser<TItem>(comparer);
        }
    }
}
