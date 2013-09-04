// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionUtils.cs" company="Labo">
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
//   Defines the CollectionUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    using Labo.Common.Dynamic;

    /// <summary>
    /// Collection Utility class.
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
        /// <returns>Converted list.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// list
        /// or
        /// converter
        /// </exception>
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
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="values">The values.</param>
        /// <exception cref="System.ArgumentNullException">
        /// list
        /// or
        /// values
        /// </exception>
        public static void AddRange<TItem>(IList<TItem> list, IList<TItem> values)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (values == null) throw new ArgumentNullException("values");

            for (int i = 0; i < values.Count; i++)
            {
                TItem local = values[i];
                list.Add(local);
            }
        }

        /// <summary>
        /// Projects the specified list.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="func">The project function.</param>
        /// <returns>Projected object.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// list
        /// or
        /// func
        /// </exception>
        public static TKey[] Project<TItem, TKey>(IList<TItem> list, Func<TItem, TKey> func)
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
        /// <typeparam name="TItem">Collection item type.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified collection]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<TItem>(ICollection<TItem> collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// Determines whether [is null or empty] [the specified array].
        /// </summary>
        /// <typeparam name="TItem">Array item type.</typeparam>
        /// <param name="array">The array.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified array]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<TItem>(TItem[] array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>
        /// To the data table.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Converted data table.</returns>
        /// <exception cref="System.ArgumentNullException">list</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static DataTable ToDataTable<TItem>(IList<TItem> list, CultureInfo culture = null)
            where TItem : IDictionary
        {
            if (list == null) throw new ArgumentNullException("list");

            culture = CultureUtils.GetCurrentCultureIfNull(culture);

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

                // Select column names
                HashSet<string> columns = new HashSet<string>();
                for (int i = 0; i < list.Count; i++)
                {
                    TItem item = list[i];
                    foreach (DictionaryEntry dictionaryEntry in item)
                    {
                        columns.Add(ConvertUtils.ChangeType<string>(dictionaryEntry.Key, culture));
                    }
                }

                result.Columns.AddRange(columns.Select(c => new DataColumn(c, typeof(object))).ToArray());

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
        /// <param name="list">The list.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Converted data table.</returns>
        /// <exception cref="System.ArgumentNullException">list</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static DataTable ToDataTable(ICollection<DynamicDictionary> list, CultureInfo culture = null)
        {
            if (list == null) throw new ArgumentNullException("list");

            culture = CultureUtils.GetCurrentCultureIfNull(culture);

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
                result.Columns.AddRange(columnNames.Select(c => new DataColumn(c, typeof(object))).ToArray());

                foreach (DynamicDictionary item in list)
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

        /// <summary>
        /// Adds the row.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="item">The item.</param>
        /// <param name="culture">The culture.</param>
        private static void AddRow(DataTable table, IDictionary item, CultureInfo culture = null)
        {
            DataRow row = table.NewRow();
            foreach (var key in item.Keys)
            {
                string columnName = ConvertUtils.ChangeType<string>(key, CultureUtils.GetCurrentCultureIfNull(culture));
                row[columnName] = item[key];
            }

            table.Rows.Add(row);
        }
    }
}