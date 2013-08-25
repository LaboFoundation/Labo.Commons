using System;

using Labo.Common.Utils;

using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class TypeUtilsFixture
    {
        [Test, Sequential]
        public void IsNumberType(
            [Values(typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal), typeof(bool), typeof(DateTime))]Type type,
            [Values(true, true, true, true, false, false, false, false, false)]bool expected)
        {
            bool result = TypeUtils.IsNumberType(type);

            Assert.AreEqual(expected, result);
        }

        [Test, Sequential]
        public void IsNumberTypeForNullables(
            [Values(typeof(byte?), typeof(short?), typeof(int?), typeof(long?), typeof(float?), typeof(double?), typeof(decimal?), typeof(bool?), typeof(DateTime?))]Type type,
            [Values(true, true, true, true, false, false, false, false, false)]bool expected)
        {
            bool result = TypeUtils.IsNumberType(type, true);

            Assert.AreEqual(expected, result);
        }
    }
}
