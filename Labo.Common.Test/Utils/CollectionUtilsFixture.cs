using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Labo.Common.Dynamic;
using Labo.Common.Utils;

using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class CollectionUtilsFixture
    {
        private class Item
        {
            public string Key { get; set; }

            public string Value { get; set; }
        }

        [Test]
        public void IsNullOrEmpty()
        {
            int[] ints = new int[0];
            Assert.IsTrue(CollectionUtils.IsNullOrEmpty(ints));

            int[] nullInts = null;
            Assert.IsTrue(CollectionUtils.IsNullOrEmpty(nullInts));

            Assert.IsFalse(CollectionUtils.IsNullOrEmpty(new int[2]));
        }

        [Test]
        public void IsNullOrEmptyCollection()
        {
            IList<int> ints = new List<int>();
            Assert.IsTrue(CollectionUtils.IsNullOrEmpty(ints));

            IList<int> nullInts = null;
            Assert.IsTrue(CollectionUtils.IsNullOrEmpty(nullInts));

            Assert.IsFalse(CollectionUtils.IsNullOrEmpty(new List<int> { 1, 2 }));
        }

        [Test]
        public void AddRange()
        {
            List<int> ints = new List<int>();
            CollectionUtils.AddRange(ints, new int[4]);

            Assert.AreEqual(4, ints.Count);

            ints = new List<int> {1, 2};
            CollectionUtils.AddRange(ints, new List<int> { 3, 4 });

            Assert.AreEqual(4, ints.Count);
            Assert.AreEqual(1, ints[0]);
            Assert.AreEqual(2, ints[1]);
            Assert.AreEqual(3, ints[2]);
            Assert.AreEqual(4, ints[3]);
        }

        [Test]
        public void AddRangeThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CollectionUtils.AddRange(null, new List<int>()));
            Assert.Throws<ArgumentNullException>(() => CollectionUtils.AddRange(new List<int>(), null));
        }

        [Test]
        public void Project()
        {
            List<Item> items = new List<Item>
                                  {
                                      new Item { Key = "Key1", Value = "Value1" }, 
                                      new Item { Key = "Key2", Value = "Value2" }, 
                                      new Item { Key = "Key3", Value = "Value3" }
                                  };

           string[] keys = CollectionUtils.Project(items, x => x.Key);
           string[] values = CollectionUtils.Project(items, x => x.Value);

            Assert.AreEqual(3, keys.Length);
            Assert.AreEqual(3, values.Length);

            Assert.AreEqual("Key1", keys[0]);
            Assert.AreEqual("Key2", keys[1]);
            Assert.AreEqual("Key3", keys[2]);

            Assert.AreEqual("Value1", values[0]);
            Assert.AreEqual("Value2", values[1]);
            Assert.AreEqual("Value3", values[2]);
        }

        [Test]
        public void ProjectThrowsArgumentNullException()
        {
            Func<int, string> func = x => x.ToStringInvariant();
            Func<int, string> nullFunc = null;
            
            Assert.Throws<ArgumentNullException>(() => CollectionUtils.Project(null, func));
            Assert.Throws<ArgumentNullException>(() => CollectionUtils.Project(new List<int>(), nullFunc));
        }

        [Test]
        public void ConverTo()
        {
            List<Item> items = new List<Item>
                                  {
                                      new Item { Key = "1", Value = "Value1" }, 
                                      new Item { Key = "2", Value = "Value2" }, 
                                      new Item { Key = "3", Value = "Value3" }
                                  };

            IList<int> convertedList = CollectionUtils.ConvertTo(items, x => int.Parse(x.Key));

            Assert.AreEqual(3, convertedList.Count);

            Assert.AreEqual(1, convertedList[0]);
            Assert.AreEqual(2, convertedList[1]);
            Assert.AreEqual(3, convertedList[2]);
        }

        [Test]
        public void ConverToEmptyList()
        {
            List<Item> items = new List<Item>();

            IList<int> convertedList = CollectionUtils.ConvertTo(items, x => int.Parse(x.Key));

            Assert.AreEqual(0, convertedList.Count);
        }

        [Test]
        public void ConverToThrowsArgumentNullException()
        {
            Func<int, string> func = x => x.ToStringInvariant();
            Func<int, string> nullFunc = null;

            Assert.Throws<ArgumentNullException>(() => CollectionUtils.ConvertTo(null, func));
            Assert.Throws<ArgumentNullException>(() => CollectionUtils.ConvertTo(new List<int>(), nullFunc));
        }

        [Test]
        public void ToDataTable()
        {
            DataTable table = CollectionUtils.ToDataTable(new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> {{"ID", 1}, {"Name", "Item1"}, {"Price", 5.6M}, {"IsActive", true}}, 
                    new Dictionary<string, object> {{"ID", 2}, {"Name", "Item2"}, {"Price", 10M},  {"IsActive", true}}, 
                    new Dictionary<string, object> {{"ID", 3}, {"Name", "Item3"}, {"Price", 15M}}
                }, CultureInfo.InvariantCulture);

            AssertToDataTable(table);
        }

        [Test]
        public void ToDataTableDynamicDictionary()
        {
            ICollection<DynamicDictionary> list = new List<DynamicDictionary>
                {
                    new DynamicDictionary {{"ID", 1}, {"Name", "Item1"}, {"Price", 5.6M}, {"IsActive", true}},
                    new DynamicDictionary {{"ID", 2}, {"Name", "Item2"}, {"Price", 10M}, {"IsActive", true}},
                    new DynamicDictionary {{"ID", 3}, {"Name", "Item3"}, {"Price", 15M}}
                };
            DataTable table = CollectionUtils.ToDataTable(list, CultureInfo.InvariantCulture);

            AssertToDataTable(table);
        }

        private static void AssertToDataTable(DataTable table)
        {
            Assert.AreEqual(4, table.Columns.Count);
            Assert.AreEqual("ID", table.Columns[0].ColumnName);
            Assert.AreEqual("Name", table.Columns[1].ColumnName);
            Assert.AreEqual("Price", table.Columns[2].ColumnName);
            Assert.AreEqual("IsActive", table.Columns[3].ColumnName);

            Assert.AreEqual(3, table.Rows.Count);
            Assert.AreEqual(1, table.Rows[0]["ID"]);
            Assert.AreEqual("Item1", table.Rows[0]["Name"]);
            Assert.AreEqual(5.6M, table.Rows[0]["Price"]);
            Assert.AreEqual(true, table.Rows[0]["IsActive"]);

            Assert.AreEqual(2, table.Rows[1]["ID"]);
            Assert.AreEqual("Item2", table.Rows[1]["Name"]);
            Assert.AreEqual(10M, table.Rows[1]["Price"]);
            Assert.AreEqual(true, table.Rows[1]["IsActive"]);

            Assert.AreEqual(3, table.Rows[2]["ID"]);
            Assert.AreEqual("Item3", table.Rows[2]["Name"]);
            Assert.AreEqual(15M, table.Rows[2]["Price"]);
            Assert.AreEqual(DBNull.Value, table.Rows[2]["IsActive"]);
        }

        [Test]
        public void ToDataTableEmptyList()
        {
            DataTable table = CollectionUtils.ToDataTable(new List<Dictionary<string, object>>(), CultureInfo.InvariantCulture);

            Assert.AreEqual(0, table.Columns.Count);
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        public void ToDataTableDynamicDictionaryEmptyList()
        {
            ICollection<DynamicDictionary> list = new List<DynamicDictionary>();
            DataTable table = CollectionUtils.ToDataTable(list, CultureInfo.InvariantCulture);

            Assert.AreEqual(0, table.Columns.Count);
            Assert.AreEqual(0, table.Rows.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDataTableNullListThrowsArgumentNullException()
        {
            List<IDictionary> dictionary = null;
            CollectionUtils.ToDataTable(dictionary, CultureInfo.InvariantCulture);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDataTableDynamicDictionaryNullListThrowsArgumentNullException()
        {
            CollectionUtils.ToDataTable(null, CultureInfo.InvariantCulture);
        }
    }
}
