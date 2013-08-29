using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Labo.Common.Dynamic;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        /// Converts a list to another list.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="converter">The converter.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static IList<TDestination> ConvertTo<TTarget, TDestination>(IList<TTarget> list, Func<TTarget, TDestination> converter)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (converter == null) throw new ArgumentNullException("converter");

            IList<TDestination> returnValue = new List<TDestination>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                TTarget item = list[i];
                returnValue.Add(converter(item));
            }
            return returnValue;
        }

        /// <summary>
        /// Adds the range to the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="values">The values.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddRange<T>(IList<T> list, IList<T> values)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (values == null) throw new ArgumentNullException("values");

            for (int i = 0; i < values.Count; i++)
            {
                T local = values[i];
                list.Add(local);
            }
        }

        /// <summary>
        /// Projects the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="func">The func.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static TKey[] Project<T, TKey>(IList<T> list, Func<T, TKey> func)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (func == null) throw new ArgumentNullException("func");

            int count = list.Count;
            TKey[] localArray = new TKey[count];
            for (int i = 0; i < count; i++)
            {
                localArray[i] = func(list[i]);
            }
            return localArray;
        }

        /// <summary>
        /// Determines whether [is null or empty] [the specified collection].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified collection]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// Determines whether [is null or empty] [the specified array].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified array]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(T[] array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>
        /// To the data table.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="culture">The culture.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"), 
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static DataTable ToDataTable<TItem, TKey, TValue>(IList<TItem> list, CultureInfo culture = null)
            where TItem : IDictionary<TKey, TValue>
        {
            if (list == null) throw new ArgumentNullException("list");

            culture = (culture ?? CultureInfo.CurrentCulture);

            DataTable result = null;
            bool succeded = false;
            try
            {
                result = new DataTable
                {
                    Locale = culture
                };

                if (list.Count == 0)
                {
                    succeded = true;
                    return result;
                }

                IEnumerable<string> columnNames = list.SelectMany(dict => dict.Keys.Select(x => ConvertUtils.ChangeType<string>(x))).Distinct();
                result.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());

                for (int i = 0; i < list.Count; i++)
                {
                    TItem item = list[i];
                    AddRow(result, item, culture);
                }

                succeded = true;
            }
            finally 
            {
                if (!succeded && result != null)
                {
                    result.Dispose();
                    result = null;
                }
            }

            return result;
        }

        /// <summary>
        /// To the data table.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="culture">The cultrue.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter"),
         System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static DataTable ToDataTable<TKey, TValue>(ICollection<DynamicDictionary> list, CultureInfo culture = null)
        {
            if (list == null) throw new ArgumentNullException("list");

            culture = (culture ?? CultureInfo.CurrentCulture);

            DataTable result = null;
            bool succeded = false;
            try
            {
                result = new DataTable { Locale = culture };

                if (list.Count == 0)
                {
                    succeded = true;
                    return result;
                }

                IEnumerable<string> columnNames = list.SelectMany(dict => dict.Keys).Distinct();
                result.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());

                foreach (IDictionary<TKey, TValue> item in list)
                {
                    AddRow(result, item, culture);
                }
                succeded = true;
            }
            finally
            {
                if (!succeded && result != null)
                {
                    result.Dispose();
                    result = null;
                }
            }

            return result;
        }

        private static void AddRow<TKey, TValue>(DataTable table, IDictionary<TKey, TValue> item, CultureInfo culture = null)
        {
            DataRow row = table.NewRow();
            foreach (TKey key in item.Keys)
            {
                row[ConvertUtils.ChangeType<string>(key, culture ?? CultureInfo.CurrentCulture)] = item[key];
            }

            table.Rows.Add(row);
        }
    }
}