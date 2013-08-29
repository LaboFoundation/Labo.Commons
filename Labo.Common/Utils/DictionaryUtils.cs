using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
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
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">dictionary</exception>
        public static NameValueCollection ToNameValueCollection(IDictionary dictionary, CultureInfo culture = null)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            culture = (culture ?? CultureInfo.CurrentCulture);

            NameValueCollection result = new NameValueCollection(dictionary.Count);
            foreach (DictionaryEntry dictionaryEntry in dictionary)
            {
                result.Add(ConvertUtils.ChangeType<string>(dictionaryEntry.Key, culture), ConvertUtils.ChangeType<string>(dictionaryEntry.Value, culture));
                
            }

            return result;
        }
    }
}
