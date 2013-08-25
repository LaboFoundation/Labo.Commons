using System.Collections.Generic;

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
    }
}
