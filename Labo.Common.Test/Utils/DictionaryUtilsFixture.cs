using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Labo.Common.Utils;
using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class DictionaryUtilsFixture
    {
        [Test]
        public void GetValue()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
                                                        {
                                                            {"A", "1"},
                                                            {"B", "2"},
                                                            {"C", "3"}
                                                        };

            Assert.AreEqual("1", DictionaryUtils.GetValue(dictionary, "A", string.Empty));
            Assert.AreEqual("2", DictionaryUtils.GetValue(dictionary, "B", string.Empty));
            Assert.AreEqual("3", DictionaryUtils.GetValue(dictionary, "C", string.Empty));

            Assert.AreEqual("Default", DictionaryUtils.GetValue(dictionary, "D", "Default"));
        }

        [Test]
        public void GetValueThrowsArgumentNullException()
        {
            Func<string> func = null;

            Assert.Throws<ArgumentNullException>(() => DictionaryUtils.GetValue(null, "B", string.Empty));
            Assert.Throws<ArgumentNullException>(() => DictionaryUtils.GetValue(new Dictionary<string, string>(), "B", func));
        }

        private readonly object[] ToNameValueCollectionKeyValueSource = new object[]
            {
                1, int.MaxValue, 42235.5252352F, 235235252525L, short.MaxValue, null
            };

        private readonly object[] ToNameValueCollectionValueValueSource = new object[]
            {
                Color.Blue, DateTime.MaxValue, Guid.NewGuid(), "Test", true, null
            };

        private readonly object[] ToNameValueCollectionCultureSource = new object[]
            {
                null, CultureInfo.InvariantCulture, new CultureInfo("tr-TR")
            };

        [Test]
        public void ToNameValueCollection(
            [ValueSource("ToNameValueCollectionKeyValueSource")]object key, 
            [ValueSource("ToNameValueCollectionValueValueSource")]object value,
            [ValueSource("ToNameValueCollectionCultureSource")]CultureInfo culture)
        {
            Type dictionaryType = typeof (Dictionary<,>).MakeGenericType(key == null ? typeof(object) : key.GetType(), value == null ? typeof(object) : value.GetType());
            IDictionary dictionary = (IDictionary) Activator.CreateInstance(dictionaryType);
            dictionaryType.GetMethod("Add").Invoke(dictionary, new[]{key ?? string.Empty, value});

            NameValueCollection nameValueCollection = DictionaryUtils.ToNameValueCollection(dictionary, culture);
           
            Assert.AreEqual(1, nameValueCollection.Count);
            Assert.IsTrue(nameValueCollection.Keys.Cast<string>().Contains(Convert.ToString(key, culture)));
            Assert.AreEqual(Convert.ToString(value, culture), nameValueCollection[Convert.ToString(key, culture)]);
        }

        [Test]
        public void ToNameValueCollection()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>
                                                        {
                                                            {"A", 1},
                                                            {"B", 2},
                                                            {"C", 3}
                                                        };

            NameValueCollection nameValueCollection = DictionaryUtils.ToNameValueCollection(dictionary);

            Assert.AreEqual(3, nameValueCollection.Count);
            Assert.AreEqual("1", nameValueCollection["A"]);
            Assert.AreEqual("2", nameValueCollection["B"]);
            Assert.AreEqual("3", nameValueCollection["C"]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToNameValueCollectionThrowArgumentNullException()
        {
           DictionaryUtils.ToNameValueCollection(null);
        }
    }
}
