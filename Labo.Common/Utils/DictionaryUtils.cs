// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryUtils.cs" company="Labo">
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
//   Defines the DictionaryUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;

    /// <summary>
    /// Dictionary utility class.
    /// </summary>
    public static class DictionaryUtils
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Value.</returns>
        public static TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            return GetValue(dictionary, key, () => defaultValue);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Value.</returns>
        public static TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValue)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");
            if (defaultValue == null) throw new ArgumentNullException("defaultValue");

            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                return defaultValue();
            }

            return value;
        }

        /// <summary>
        /// To the name value collection.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="culture">The culture for string conversion.</param>
        /// <returns>NameValueCollection.</returns>
        /// <exception cref="System.ArgumentNullException">dictionary</exception>
        public static NameValueCollection ToNameValueCollection(IDictionary dictionary, CultureInfo culture = null)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            culture = CultureUtils.GetCurrentCultureIfNull(culture);

            NameValueCollection result = new NameValueCollection(dictionary.Count);
            foreach (DictionaryEntry dictionaryEntry in dictionary)
            {
                result.Add(ConvertUtils.ChangeType<string>(dictionaryEntry.Key, culture), ConvertUtils.ChangeType<string>(dictionaryEntry.Value, culture));
            }

            return result;
        }
    }
}
