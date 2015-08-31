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
            [Values(typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal), typeof(bool), typeof(string))]Type type,
            [Values(0, 0, 0, 0, 0, 0, 0, false, null)]object expected)
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

        [Test]
        public void GetTypeTest()
        {
            Assert.AreEqual(typeof(object), TypeUtils.GetType(null));

            string s = null;
            Assert.AreEqual(typeof(string), TypeUtils.GetType(s));
        }

        [Test]
        public void GetDefaultValue()
        {
            Assert.AreEqual(0, TypeUtils.GetDefaultValue(1));
            Assert.AreEqual(null, TypeUtils.GetDefaultValue(null));

            string s = null;
            Assert.AreEqual(null, TypeUtils.GetDefaultValue(s));
        }

        [Test]
        public void IsImplicitNumericConvertible_Char()
        {
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(ushort)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(ushort)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(int)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(uint)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(long)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(ulong)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(float)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(double)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(decimal)));

            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(sbyte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(byte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(char), typeof(short)));
        }

        [Test]
        public void IsImplicitNumericConvertible_SByte()
        {
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(short)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(int)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(long)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(float)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(double)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(decimal)));

            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(char)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(byte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(ushort)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(uint)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(sbyte), typeof(ulong)));
        }

        [Test]
        public void IsImplicitNumericConvertible_Byte()
        {
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(short)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(ushort)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(uint)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(ulong)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(int)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(long)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(float)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(double)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(decimal)));

            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(char)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(byte), typeof(sbyte)));
        }

        [Test]
        public void IsImplicitNumericConvertible_Short()
        {
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(int)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(long)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(float)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(double)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(decimal)));

            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(char)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(sbyte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(byte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(ushort)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(uint)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(short), typeof(ulong)));
        }

        [Test]
        public void IsImplicitNumericConvertible_UShort()
        {
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(uint)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(ulong)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(int)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(long)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(float)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(double)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(decimal)));

            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(char)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(sbyte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(byte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ushort), typeof(short)));
        }

        [Test]
        public void IsImplicitNumericConvertible_Int()
        {
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(long)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(float)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(double)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(decimal)));

            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(char)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(sbyte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(byte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(short)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(ushort)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(uint)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(int), typeof(ulong)));
        }

        [Test]
        public void IsImplicitNumericConvertible_UInt()
        {
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(ulong)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(long)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(float)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(double)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(decimal)));

            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(char)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(sbyte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(byte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(short)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(ushort)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(uint), typeof(int)));
        }

        [Test]
        public void IsImplicitNumericConvertible_Long()
        {
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(float)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(double)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(decimal)));

            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(char)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(sbyte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(byte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(short)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(ushort)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(int)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(uint)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(long), typeof(ulong)));
        }

        [Test]
        public void IsImplicitNumericConvertible_ULong()
        {
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(float)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(double)));
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(decimal)));

            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(char)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(sbyte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(byte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(short)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(ushort)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(int)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(uint)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(ulong), typeof(long)));
        }

        [Test]
        public void IsImplicitNumericConvertible_Float()
        {
            Assert.IsTrue(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(double)));

            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(char)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(sbyte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(byte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(short)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(ushort)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(int)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(uint)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(long)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(ulong)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(float), typeof(decimal)));
        }

        [Test]
        public void IsImplicitNumericConvertible_Decimal()
        {
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(decimal), typeof(char)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(decimal), typeof(sbyte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(decimal), typeof(byte)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(decimal), typeof(short)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(decimal), typeof(ushort)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(decimal), typeof(int)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(decimal), typeof(uint)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(decimal), typeof(long)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(decimal), typeof(ulong)));
            Assert.IsFalse(TypeUtils.IsImplicitNumericConvertible(typeof(decimal), typeof(double)));
        }
    }
}
