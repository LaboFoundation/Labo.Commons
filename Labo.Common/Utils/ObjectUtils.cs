// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectUtils.cs" company="Labo">
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
//   Defines the ObjectUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Object utility class.
    /// </summary>
    public static class ObjectUtils
    {
        /// <summary>
        /// Determines whether the specified object is null.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="defaultValueCreator">The default value creator.</param>
        /// <returns>object.</returns>
        /// <exception cref="System.ArgumentNullException">defaultValueCreator</exception>
        public static TObject IsNull<TObject>(TObject @object, Func<TObject> defaultValueCreator)
            where TObject : class
        {
            if (defaultValueCreator == null) throw new ArgumentNullException("defaultValueCreator");

            if (@object == null)
            {
                return defaultValueCreator();
            }

            return @object;
        }

        /// <summary>
        /// Casts the specified object.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="object">The object.</param>
        /// <returns>casted object.</returns>
        public static TObject Cast<TObject>(object @object)
        {
            return (TObject)@object;
        }

        /// <summary>
        /// To the string invariant.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns>string</returns>
        public static string ToStringInvariant(object @object)
        {
            return Convert.ToString(@object, CultureInfo.InvariantCulture);
        }
    }
}
