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

        [Test]
        public void IsNumberTypeThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => TypeUtils.IsNumberType(null));
        }

        [Test, Sequential]
        public void IsNumberTypeForNullables(
            [Values(typeof(byte?), typeof(short?), typeof(int?), typeof(long?), typeof(float?), typeof(double?), typeof(decimal?), typeof(bool?), typeof(DateTime?))]Type type,
            [Values(true, true, true, true, false, false, false, false, false)]bool expected)
        {
            bool result = TypeUtils.IsNumberType(type, true);

            Assert.AreEqual(expected, result);
        }

        [Test, Sequential]
        public void IsNumericType(
            [Values(typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal), typeof(bool), typeof(DateTime))]Type type,
            [Values(true, true, true, true, true, true, true, false, false)]bool expected)
        {
            bool result = TypeUtils.IsNumericType(type);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void IsNumericTypeThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => TypeUtils.IsNumericType(null));
        }

        [Test, Sequential]
        public void IsNumericForNullables(
            [Values(typeof(byte?), typeof(short?), typeof(int?), typeof(long?), typeof(float?), typeof(double?), typeof(decimal?), typeof(bool?), typeof(DateTime?))]Type type,
            [Values(true, true, true, true, true, true, true, false, false)]bool expected)
        {
            bool result = TypeUtils.IsNumericType(type, true);

            Assert.AreEqual(expected, result);
        }

        [Test, Sequential]
        public void IsFloatingPointNumberType(
            [Values(typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal), typeof(bool), typeof(DateTime))]Type type,
            [Values(false, false, false, false, true, true, true, false, false)]bool expected)
        {
            bool result = TypeUtils.IsFloatingPointNumberType(type);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void IsFloatingPointNumberTypeThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => TypeUtils.IsFloatingPointNumberType(null));
        }

        [Test, Sequential]
        public void IsFloatingPointNumberTypeForNullables(
            [Values(typeof(byte?), typeof(short?), typeof(int?), typeof(long?), typeof(float?), typeof(double?), typeof(decimal?), typeof(bool?), typeof(DateTime?))]Type type,
            [Values(false, false, false, false, true, true, true, false, false)]bool expected)
        {
            bool result = TypeUtils.IsFloatingPointNumberType(type, true);

            Assert.AreEqual(expected, result);
        }

        [Test, Sequential]
        public void GetDefaultValueOfType(
            [Values(typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal), typeof(bool))]Type type,
            [Values(0, 0, 0, 0, 0, 0, 0, false)]object expected)
        { 
            Assert.AreEqual(expected, TypeUtils.GetDefaultValueOfType(type));
        }

        [Test]
        public void GetDefaultValueOfTypeThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => TypeUtils.GetDefaultValueOfType(null));
        }

        [Test, Sequential]
        public void GetDefaultValueOfTypeForNullables(
            [Values(typeof(byte?), typeof(short?), typeof(int?), typeof(long?), typeof(float?), typeof(double?), typeof(decimal?), typeof(bool?), typeof(DateTime?))]Type type,
            [Values(null, null, null, null, null, null, null, null, null)]object expected)
        {
            Assert.AreEqual(expected, TypeUtils.GetDefaultValueOfType(type));
        }

        [Test, Sequential]
        public void GetTypeTest()
        {
            Assert.AreEqual(typeof(object), TypeUtils.GetType(null));

            string s = null;
            Assert.AreEqual(typeof(string), TypeUtils.GetType(s));
        }

        [Test, Sequential]
        public void GetDefaultValue()
        {
            Assert.AreEqual(0, TypeUtils.GetDefaultValue(1));
            Assert.AreEqual(null, TypeUtils.GetDefaultValue(null));

            string s = null;
            Assert.AreEqual(null, TypeUtils.GetDefaultValue(s));
        }
    }
}
