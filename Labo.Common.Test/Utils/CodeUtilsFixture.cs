using System;
using Labo.Common.Utils;
using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class CodeUtilsFixture
    {
        [Test]
        public void TryCatch()
        {
            Assert.DoesNotThrow(() => CodeUtils.TryCatch(() => { throw new Exception(); }));
            Assert.Throws<ArgumentNullException>(() => CodeUtils.TryCatch(null));
            Assert.AreEqual(false,  CodeUtils.TryCatch(() => { throw new Exception(); }));
            Assert.AreEqual(true, CodeUtils.TryCatch(() => { }));

            Exception exceptionToThrow = new Exception();
            CodeUtils.TryCatch(() => { throw exceptionToThrow; }, x => Assert.AreSame(exceptionToThrow, x));
        }
    }
}
