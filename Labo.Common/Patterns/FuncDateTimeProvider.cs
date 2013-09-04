// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncDateTimeProvider.cs" company="Labo">
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
//   Defines the FuncDateTimeProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Patterns
{
    using System;

    /// <summary>
    /// Date time provider function class.
    /// </summary>
    public sealed class FuncDateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// The date time function.
        /// </summary>
        private readonly Func<DateTime> m_FuncDateTime;

        /// <summary>
        /// Gets the UTC now.
        /// </summary>
        /// <value>
        /// The UTC now.
        /// </value>
        public DateTime UtcNow
        {
            get
            {
                return DateTime.SpecifyKind(m_FuncDateTime(), DateTimeKind.Utc);
            }
        }

        /// <summary>
        /// Gets the now.
        /// </summary>
        /// <value>
        /// The now.
        /// </value>
        public DateTime Now
        {
            get
            {
                return m_FuncDateTime();
            }
        }

        /// <summary>
        /// Gets the today.
        /// </summary>
        /// <value>
        /// The today.
        /// </value>
        public DateTime Today
        {
            get
            {
                return m_FuncDateTime().Date;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncDateTimeProvider"/> class.
        /// </summary>
        /// <param name="funcDateTime">The date time function.</param>
        /// <exception cref="System.ArgumentNullException">funcDateTime</exception>
        public FuncDateTimeProvider(Func<DateTime> funcDateTime)
        {
            if (funcDateTime == null) throw new ArgumentNullException("funcDateTime");

            m_FuncDateTime = funcDateTime;
        }
    }
}