using System;
using System.Collections.Generic;
using System.Collections.Specialized;

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
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            NameValueCollection result = new NameValueCollection(dictionary.Count);
            foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
            {
                result.Add(ConvertUtils.ChangeType<string>(keyValuePair.Key), ConvertUtils.ChangeType<string>(keyValuePair.Value));
            }

            return result;
        }
    }
}
