using System.Collections.Generic;

using Labo.Common.Comparer;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Performs the specified action for each element of the System.Collections.Generic.IEnumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The target collection.</param>
        /// <param name="action">The action to be performed foreach element of the collection.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in collection)
            {
                action(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            return new ComparerReverser<T>(comparer);
        }
    }
}
