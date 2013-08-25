using System;

using Labo.Common.Exceptions;
using Labo.Common.Utils;

using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class ExceptionUtilsFixture
    {
        [Test]
        public void Find()
        {
            CoreLevelException coreLevelException = new CoreLevelException();
            UserLevelException userLevelException = new UserLevelException(coreLevelException);

            CoreLevelException foundCoreLevelException = ExceptionUtils.Find<CoreLevelException>(userLevelException);

            Assert.AreSame(coreLevelException, foundCoreLevelException);
        }

        [Test]
        public void FindByBaseType()
        {
            CriticalUserLevelException criticalUserLevelException = new CriticalUserLevelException();
            UserLevelException userLevelException = new UserLevelException(criticalUserLevelException);

            Assert.AreSame(userLevelException, ExceptionUtils.Find<Exception>(userLevelException));
            Assert.AreSame(userLevelException, ExceptionUtils.Find<BaseUserLevelException>(userLevelException));
            Assert.AreSame(criticalUserLevelException, ExceptionUtils.Find<CriticalUserLevelException>(userLevelException));
        }

        [Test]
        public void FindNotFoundType()
        {
            CriticalUserLevelException criticalUserLevelException = new CriticalUserLevelException();
            UserLevelException userLevelException = new UserLevelException(criticalUserLevelException);

            Assert.AreEqual(null, ExceptionUtils.Find<CoreLevelException>(userLevelException));
        }
    }
}
