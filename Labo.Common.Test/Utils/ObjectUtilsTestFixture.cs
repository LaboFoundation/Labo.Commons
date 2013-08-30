using System;
using Labo.Common.Utils;
using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class ObjectUtilsTestFixture
    {
        [Test]
        public void IsNull()
        {
            string s = null;
            Assert.AreEqual("aaa", ObjectUtils.IsNull(s, () => "aaa"));
            Assert.AreEqual("bbb", ObjectUtils.IsNull("bbb", () => "aaa"));
        }

        [Test]
        public void IsNullThrowsException()
        {
            string s = null;
            Assert.Throws<ArgumentNullException>(() => ObjectUtils.IsNull(s, null));
        }

        [Test]
        public void Cast()
        {
            object s = "aaa";
            Assert.AreEqual("aaa", ObjectUtils.Cast<string>(s));
        }
    }
}
