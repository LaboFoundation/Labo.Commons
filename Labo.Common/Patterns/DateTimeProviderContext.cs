// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeProviderContext.cs" company="Labo">
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
//   Defines the DateTimeProviderContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Patterns
{
    using System;

    /// <summary>
    /// Date time provider context.
    /// </summary>
    public sealed class DateTimeProviderContext : IDisposable
    {
        /// <summary>
        /// The date time provider
        /// </summary>
        private IDateTimeProvider m_DateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeProviderContext"/> class.
        /// </summary>
        /// <param name="dateTimeProvider">The date time provider.</param>
        /// <exception cref="System.ArgumentNullException">dateTimeProvider</exception>
        public DateTimeProviderContext(IDateTimeProvider dateTimeProvider)
        {
            if (dateTimeProvider == null) throw new ArgumentNullException("dateTimeProvider");

            m_DateTimeProvider = dateTimeProvider;

            DateTimeProvider.SetDateTimeProvider(m_DateTimeProvider);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            DateTimeProvider.ResetDateTimeProvider();

            m_DateTimeProvider = null;
        }
    }
}