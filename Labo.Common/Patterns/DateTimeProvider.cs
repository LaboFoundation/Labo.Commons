// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeProvider.cs" company="Labo">
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
//   Defines the DateTimeProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Patterns
{
    /// <summary>
    /// Date time provider class.
    /// </summary>
    public static class DateTimeProvider
    {
        /// <summary>
        /// The date time provider object.
        /// </summary>
        private static IDateTimeProvider s_DateTimeProvider = new DefaultDateTimeProvider();

        /// <summary>
        /// Gets the current DateTimeProvider.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static IDateTimeProvider Current
        {
            get { return s_DateTimeProvider; }
        }

        /// <summary>
        /// Resets the date time provider to its default value.
        /// </summary>
        internal static void ResetDateTimeProvider()
        {
            lock (s_DateTimeProvider)
            {
                s_DateTimeProvider = new DefaultDateTimeProvider();                
            }
        }

        /// <summary>
        /// Sets the date time provider.
        /// </summary>
        /// <param name="dateTimeProvider">The date time provider.</param>
        internal static void SetDateTimeProvider(IDateTimeProvider dateTimeProvider)
        {
            lock (s_DateTimeProvider)
            {
                s_DateTimeProvider = dateTimeProvider;
            }
        }
    }
}