using System;
using System.Collections.Generic;

namespace Labo.Common.Comparer
{
    public sealed class ComparerReverser<T> : IComparer<T>
    {
        private readonly IComparer<T> m_WrappedComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparerReverser{T}" /> class.
        /// </summary>
        /// <param name="wrappedComparer">The wrapped comparer.</param>
        /// <exception cref="System.ArgumentNullException">wrappedComparer</exception>
        public ComparerReverser(IComparer<T> wrappedComparer)
        {
            if (wrappedComparer == null)
            {
                throw new ArgumentNullException("wrappedComparer");
            }

            m_WrappedComparer = wrappedComparer;
        }

        /// <summary>
        /// Compares the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            return m_WrappedComparer.Compare(y, x);
        }
    }
}
