// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemExtensions.cs" company="Labo">
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
//   System extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace System
{
    using System.Globalization;

    using Labo.Common.Utils;

    /// <summary>
    /// System extension methods.
    /// </summary>
    public static class SystemExtensions
    {
        /// <summary>
        /// Determines whether [is null or empty] [the specified string].
        /// </summary>
        /// <param name="string">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string);
        }

        /// <summary>
        /// Determines whether [is null or white space] [the specified string].
        /// </summary>
        /// <param name="string">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is null or white space] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string @string)
        {
            return string.IsNullOrWhiteSpace(@string);
        }

        /// <summary>
        ///  Determines whether two specified <see cref="T:System.String"/> objects have the same value using ordinal ignore case comparison
        /// </summary>
        /// <param name="a">The first string value.</param>
        /// <param name="b">The second string value.</param>
        /// <returns>true if the value of the <paramref name="a"/> parameter is equal to the value of the <paramref name="b"/> parameter; otherwise, false.</returns>
        public static bool EqualsOrdinalIgnoreCase(this string a, string b)
        {
            return StringUtils.EqualsOrdinalIgnoreCase(a, b);
        }

        /// <summary>
        /// Converts specified object to the string using invariant culture.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns>string value of the specified object.</returns>
        public static string ToStringInvariant(this object @object)
        {
            return ObjectUtils.ToStringInvariant(@object);
        }

        /// <summary>
        /// Formats string value.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <param name="args">The args.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatWith(this string @string, params object[] args)
        {
            return FormatWith(@string, CultureInfo.CurrentCulture, args);
        }

        /// <summary>
        /// Formats string value.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="args">The args.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatWith(this string @string, IFormatProvider formatProvider, params object[] args)
        {
            return string.Format(formatProvider, @string, args);
        }
    }
}
