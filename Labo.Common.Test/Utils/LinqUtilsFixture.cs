using System;
using Labo.Common.Utils;

using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class LinqUtilsFixture
    {
        private class TestClass
        {
            private string m_Name2;

            public string Name1 { get; set; }

            public string Name2 { get { return m_Name2; } }

            public string Name3;

            public string Method1()
            {
                return string.Empty;
            }
        }

        [Test]
        public void GetProperyName()
        {
            Assert.AreEqual("Name1", LinqUtils.GetMemberName<TestClass, string>(x => x.Name1));
            Assert.AreEqual("Name2", LinqUtils.GetMemberName<TestClass, string>(x => x.Name2));
            Assert.AreEqual("Name3", LinqUtils.GetMemberName<TestClass, object>(x => x.Name3));
            Assert.AreEqual("Method1", LinqUtils.GetMemberName<TestClass, string>(x => x.Method1()));

            Assert.Throws<ArgumentNullException>(() => LinqUtils.GetMemberName<TestClass, string>(null));
        }
    }
}
