using System;
using System.Globalization;
using System.Threading;
using Labo.Common.Utils;

using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class ConvertUtilsFixture
    {
        public enum TestEnum
        {
            Value1,
            Value2,
            Value3
        }

        [Test]
        public void EnsureCanConvertFromStringToOtherTypesInvariantCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            string nullString = null;

            Assert.AreEqual(int.MaxValue, ConvertUtils.ChangeType<int>(int.MaxValue.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual(int.MaxValue, ConvertUtils.ChangeType<int?>(int.MaxValue.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual(new int?(), ConvertUtils.ChangeType<int?>(string.Empty));
            Assert.AreEqual(new int?(), ConvertUtils.ChangeType<int?>(nullString));

            Assert.AreEqual(long.MaxValue, ConvertUtils.ChangeType<long>(long.MaxValue.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual(long.MaxValue, ConvertUtils.ChangeType<long?>(long.MaxValue.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual(new long?(), ConvertUtils.ChangeType<long?>(string.Empty));
            Assert.AreEqual(new long?(), ConvertUtils.ChangeType<long?>(nullString));

            Assert.AreEqual(short.MaxValue, ConvertUtils.ChangeType<short>(short.MaxValue.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual(short.MaxValue, ConvertUtils.ChangeType<short?>(short.MaxValue.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual(new short?(), ConvertUtils.ChangeType<short?>(string.Empty));
            Assert.AreEqual(new short?(), ConvertUtils.ChangeType<short?>(nullString));

            Assert.AreEqual(byte.MaxValue, ConvertUtils.ChangeType<byte>(byte.MaxValue.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual(byte.MaxValue, ConvertUtils.ChangeType<byte?>(byte.MaxValue.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual(new byte?(), ConvertUtils.ChangeType<byte?>(string.Empty));
            Assert.AreEqual(new byte?(), ConvertUtils.ChangeType<byte?>(nullString));

            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(bool.FalseString));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool?>(bool.FalseString));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(string.Empty));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(nullString));

            Assert.AreEqual(decimal.MaxValue, ConvertUtils.ChangeType<decimal>(decimal.MaxValue.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual(decimal.MaxValue, ConvertUtils.ChangeType<decimal?>(decimal.MaxValue.ToString(CultureInfo.InvariantCulture)));
            Assert.AreEqual(new decimal?(), ConvertUtils.ChangeType<decimal?>(string.Empty));
            Assert.AreEqual(new decimal?(), ConvertUtils.ChangeType<decimal?>(nullString));

            Assert.AreEqual(float.MaxValue, ConvertUtils.ChangeType<float>("3.40282347E+38"));
            Assert.AreEqual(float.MaxValue, ConvertUtils.ChangeType<float?>("3.40282347E+38"));
            Assert.AreEqual(new float?(), ConvertUtils.ChangeType<float?>(string.Empty));
            Assert.AreEqual(new float?(), ConvertUtils.ChangeType<float?>(nullString));

            Assert.AreEqual(DateTime.MaxValue, ConvertUtils.ChangeType<DateTime>("9999-12-31 23:59:59.9999999"));
            Assert.AreEqual(DateTime.MaxValue, ConvertUtils.ChangeType<DateTime?>("9999-12-31 23:59:59.9999999"));
            Assert.AreEqual(new DateTime?(), ConvertUtils.ChangeType<DateTime?>(string.Empty));
            Assert.AreEqual(new DateTime?(), ConvertUtils.ChangeType<DateTime?>(nullString));

            Guid guid = Guid.NewGuid();
            Assert.AreEqual(guid, ConvertUtils.ChangeType<Guid>(guid.ToString()));
            Assert.AreEqual(guid, ConvertUtils.ChangeType<Guid?>(guid.ToString()));
            Assert.AreEqual(new Guid?(), ConvertUtils.ChangeType<Guid?>(string.Empty));
            Assert.AreEqual(new Guid?(), ConvertUtils.ChangeType<Guid?>(nullString));

            const string str = "TestValue";
            Assert.AreEqual(str, ConvertUtils.ChangeType<string>(str));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(string.Empty));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(nullString));

            Assert.AreEqual(TestEnum.Value1, ConvertUtils.ChangeType<TestEnum>("Value1"));
            Assert.AreEqual(TestEnum.Value2, ConvertUtils.ChangeType<TestEnum>("value2"));
            Assert.AreEqual(TestEnum.Value3, ConvertUtils.ChangeType<TestEnum>("Value3"));
        }

        [Test]
        public void EnsureCanConvertFromStringToOtherTypesTurkishCulture()
        {
            CultureInfo culture = new CultureInfo("tr-TR");

            string nullString = null;

            Assert.AreEqual(int.MaxValue, ConvertUtils.ChangeType<int>(int.MaxValue.ToString(culture), culture));
            Assert.AreEqual(int.MaxValue, ConvertUtils.ChangeType<int?>(int.MaxValue.ToString(culture), culture));
            Assert.AreEqual(new int?(), ConvertUtils.ChangeType<int?>(string.Empty, culture));
            Assert.AreEqual(new int?(), ConvertUtils.ChangeType<int?>(nullString, culture));

            Assert.AreEqual(long.MaxValue, ConvertUtils.ChangeType<long>(long.MaxValue.ToString(culture), culture));
            Assert.AreEqual(long.MaxValue, ConvertUtils.ChangeType<long?>(long.MaxValue.ToString(culture), culture));
            Assert.AreEqual(new long?(), ConvertUtils.ChangeType<long?>(string.Empty, culture));
            Assert.AreEqual(new long?(), ConvertUtils.ChangeType<long?>(nullString, culture));

            Assert.AreEqual(short.MaxValue, ConvertUtils.ChangeType<short>(short.MaxValue.ToString(culture), culture));
            Assert.AreEqual(short.MaxValue, ConvertUtils.ChangeType<short?>(short.MaxValue.ToString(culture), culture));
            Assert.AreEqual(new short?(), ConvertUtils.ChangeType<short?>(string.Empty, culture));
            Assert.AreEqual(new short?(), ConvertUtils.ChangeType<short?>(nullString, culture));

            Assert.AreEqual(byte.MaxValue, ConvertUtils.ChangeType<byte>(byte.MaxValue.ToString(culture), culture));
            Assert.AreEqual(byte.MaxValue, ConvertUtils.ChangeType<byte?>(byte.MaxValue.ToString(culture), culture));
            Assert.AreEqual(new byte?(), ConvertUtils.ChangeType<byte?>(string.Empty, culture));
            Assert.AreEqual(new byte?(), ConvertUtils.ChangeType<byte?>(nullString, culture));

            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(bool.FalseString, culture));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool?>(bool.FalseString, culture));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(string.Empty, culture));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(nullString, culture));

            Assert.AreEqual(decimal.MaxValue, ConvertUtils.ChangeType<decimal>(decimal.MaxValue.ToString(culture), culture));
            Assert.AreEqual(decimal.MaxValue, ConvertUtils.ChangeType<decimal?>(decimal.MaxValue.ToString(culture), culture));
            Assert.AreEqual(new decimal?(), ConvertUtils.ChangeType<decimal?>(string.Empty, culture));
            Assert.AreEqual(new decimal?(), ConvertUtils.ChangeType<decimal?>(nullString, culture));

            Assert.AreEqual(float.MaxValue, ConvertUtils.ChangeType<float>("3,40282347E+38", culture));
            Assert.AreEqual(float.MaxValue, ConvertUtils.ChangeType<float?>("3,40282347E+38", culture));
            Assert.AreEqual(new float?(), ConvertUtils.ChangeType<float?>(string.Empty, culture));
            Assert.AreEqual(new float?(), ConvertUtils.ChangeType<float?>(nullString, culture));

            Assert.AreEqual(DateTime.MaxValue, ConvertUtils.ChangeType<DateTime>("9999-12-31 23:59:59.9999999", culture));
            Assert.AreEqual(DateTime.MaxValue, ConvertUtils.ChangeType<DateTime?>("9999-12-31 23:59:59.9999999", culture));
            Assert.AreEqual(new DateTime?(), ConvertUtils.ChangeType<DateTime?>(string.Empty, culture));
            Assert.AreEqual(new DateTime?(), ConvertUtils.ChangeType<DateTime?>(nullString, culture));

            Guid guid = Guid.NewGuid();
            Assert.AreEqual(guid, ConvertUtils.ChangeType<Guid>(guid.ToString(), culture));
            Assert.AreEqual(guid, ConvertUtils.ChangeType<Guid?>(guid.ToString(), culture));
            Assert.AreEqual(new Guid?(), ConvertUtils.ChangeType<Guid?>(string.Empty, culture));
            Assert.AreEqual(new Guid?(), ConvertUtils.ChangeType<Guid?>(nullString, culture));

            const string str = "TestValue";
            Assert.AreEqual(str, ConvertUtils.ChangeType<string>(str, culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(string.Empty, culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(nullString, culture));

            Assert.AreEqual(TestEnum.Value1, ConvertUtils.ChangeType<TestEnum>("Value1", culture));
            Assert.AreEqual(TestEnum.Value2, ConvertUtils.ChangeType<TestEnum>("Value2", culture));
            Assert.AreEqual(TestEnum.Value3, ConvertUtils.ChangeType<TestEnum>("Value3", culture));
        }

        [Test]
        public void EnsureCanConvertFromSameTypeToSameType()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            bool? nullBool = null;
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(true));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(false));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(nullBool));

            Assert.AreEqual(int.MaxValue, ConvertUtils.ChangeType<int>(int.MaxValue));
            Assert.AreEqual(int.MaxValue, ConvertUtils.ChangeType<int?>(int.MaxValue));
            Assert.AreEqual(new int?(), ConvertUtils.ChangeType<int?>(new int?()));

            Assert.AreEqual(long.MaxValue, ConvertUtils.ChangeType<long>(long.MaxValue));
            Assert.AreEqual(long.MaxValue, ConvertUtils.ChangeType<long?>(long.MaxValue));
            Assert.AreEqual(new long?(), ConvertUtils.ChangeType<long?>(new long?()));

            Assert.AreEqual(short.MaxValue, ConvertUtils.ChangeType<short>(short.MaxValue));
            Assert.AreEqual(short.MaxValue, ConvertUtils.ChangeType<short?>(short.MaxValue));
            Assert.AreEqual(new short?(), ConvertUtils.ChangeType<short?>(new short?()));

            Assert.AreEqual(byte.MaxValue, ConvertUtils.ChangeType<byte>(byte.MaxValue));
            Assert.AreEqual(byte.MaxValue, ConvertUtils.ChangeType<byte?>(byte.MaxValue));
            Assert.AreEqual(new byte?(), ConvertUtils.ChangeType<byte?>(new byte?()));

            Assert.AreEqual(decimal.MaxValue, ConvertUtils.ChangeType<decimal>(decimal.MaxValue));
            Assert.AreEqual(decimal.MaxValue, ConvertUtils.ChangeType<decimal?>(decimal.MaxValue));
            Assert.AreEqual(new decimal?(), ConvertUtils.ChangeType<decimal?>(new decimal?()));

            Assert.AreEqual(3.40282347F, ConvertUtils.ChangeType<float>(3.40282347F));
            Assert.AreEqual(3.40282347F, ConvertUtils.ChangeType<float?>(3.40282347F));
            Assert.AreEqual(new float?(), ConvertUtils.ChangeType<float?>(new float?()));

            Assert.AreEqual(new DateTime(2009, 11, 20), ConvertUtils.ChangeType<DateTime>(new DateTime(2009, 11, 20)));
            Assert.AreEqual(new DateTime(2009, 11, 20), ConvertUtils.ChangeType<DateTime?>(new DateTime(2009, 11, 20)));
            Assert.AreEqual(new DateTime?(), ConvertUtils.ChangeType<DateTime?>(new DateTime?()));

            Guid guid = Guid.NewGuid();
            Assert.AreEqual(guid, ConvertUtils.ChangeType<Guid>(guid));
            Assert.AreEqual(guid, ConvertUtils.ChangeType<Guid?>(guid));
            Assert.AreEqual(new Guid?(), ConvertUtils.ChangeType<Guid?>(new Guid?()));

            const string str = "TestValue";
            Assert.AreEqual(str, ConvertUtils.ChangeType<string>(str));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(string.Empty));

            Assert.AreEqual(TestEnum.Value1, ConvertUtils.ChangeType<TestEnum>(TestEnum.Value1));
            Assert.AreEqual(TestEnum.Value2, ConvertUtils.ChangeType<TestEnum>(TestEnum.Value2));
            Assert.AreEqual(TestEnum.Value3, ConvertUtils.ChangeType<TestEnum>(TestEnum.Value3));
        }

        [Test]
        public void EnsureCanConvertFromSameTypeToSameTypeTurkishCulture()
        {
            CultureInfo cultureInfo = new CultureInfo("tr-TR");
 
            bool? nullBool = null;
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(true, cultureInfo));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(false, cultureInfo));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(nullBool, cultureInfo));

            Assert.AreEqual(int.MaxValue, ConvertUtils.ChangeType<int>(int.MaxValue, cultureInfo));
            Assert.AreEqual(int.MaxValue, ConvertUtils.ChangeType<int?>(int.MaxValue, cultureInfo));
            Assert.AreEqual(new int?(), ConvertUtils.ChangeType<int?>(new int?(), cultureInfo));

            Assert.AreEqual(long.MaxValue, ConvertUtils.ChangeType<long>(long.MaxValue, cultureInfo));
            Assert.AreEqual(long.MaxValue, ConvertUtils.ChangeType<long?>(long.MaxValue, cultureInfo));
            Assert.AreEqual(new long?(), ConvertUtils.ChangeType<long?>(new long?(), cultureInfo));

            Assert.AreEqual(short.MaxValue, ConvertUtils.ChangeType<short>(short.MaxValue, cultureInfo));
            Assert.AreEqual(short.MaxValue, ConvertUtils.ChangeType<short?>(short.MaxValue, cultureInfo));
            Assert.AreEqual(new short?(), ConvertUtils.ChangeType<short?>(new short?(), cultureInfo));

            Assert.AreEqual(byte.MaxValue, ConvertUtils.ChangeType<byte>(byte.MaxValue, cultureInfo));
            Assert.AreEqual(byte.MaxValue, ConvertUtils.ChangeType<byte?>(byte.MaxValue, cultureInfo));
            Assert.AreEqual(new byte?(), ConvertUtils.ChangeType<byte?>(new byte?(), cultureInfo));

            Assert.AreEqual(decimal.MaxValue, ConvertUtils.ChangeType<decimal>(decimal.MaxValue, cultureInfo));
            Assert.AreEqual(decimal.MaxValue, ConvertUtils.ChangeType<decimal?>(decimal.MaxValue, cultureInfo));
            Assert.AreEqual(new decimal?(), ConvertUtils.ChangeType<decimal?>(new decimal?(), cultureInfo));

            Assert.AreEqual(3.40282347F, ConvertUtils.ChangeType<float>(3.40282347F, cultureInfo));
            Assert.AreEqual(3.40282347F, ConvertUtils.ChangeType<float?>(3.40282347F, cultureInfo));
            Assert.AreEqual(new float?(), ConvertUtils.ChangeType<float?>(new float?(), cultureInfo));

            Assert.AreEqual(new DateTime(2009, 11, 20), ConvertUtils.ChangeType<DateTime>(new DateTime(2009, 11, 20), cultureInfo));
            Assert.AreEqual(new DateTime(2009, 11, 20), ConvertUtils.ChangeType<DateTime?>(new DateTime(2009, 11, 20), cultureInfo));
            Assert.AreEqual(new DateTime?(), ConvertUtils.ChangeType<DateTime?>(new DateTime?(), cultureInfo));

            Guid guid = Guid.NewGuid();
            Assert.AreEqual(guid, ConvertUtils.ChangeType<Guid>(guid, cultureInfo));
            Assert.AreEqual(guid, ConvertUtils.ChangeType<Guid?>(guid, cultureInfo));
            Assert.AreEqual(new Guid?(), ConvertUtils.ChangeType<Guid?>(new Guid?(), cultureInfo));

            const string str = "TestValue";
            Assert.AreEqual(str, ConvertUtils.ChangeType<string>(str, cultureInfo));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(string.Empty, cultureInfo));

            Assert.AreEqual(TestEnum.Value1, ConvertUtils.ChangeType<TestEnum>(TestEnum.Value1, cultureInfo));
            Assert.AreEqual(TestEnum.Value2, ConvertUtils.ChangeType<TestEnum>(TestEnum.Value2, cultureInfo));
            Assert.AreEqual(TestEnum.Value3, ConvertUtils.ChangeType<TestEnum>(TestEnum.Value3, cultureInfo));
        }

        [Test]
        public void EnsureCanConvertToString()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            bool? nullBool = null;

            Assert.AreEqual(bool.TrueString, ConvertUtils.ChangeType<string>(true));
            Assert.AreEqual(bool.FalseString, ConvertUtils.ChangeType<string>(false));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(nullBool));

            Assert.AreEqual(int.MaxValue.ToString(CultureInfo.InvariantCulture), ConvertUtils.ChangeType<string>(int.MaxValue));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new int?()));

            Assert.AreEqual(int.MaxValue.ToString(CultureInfo.InvariantCulture), ConvertUtils.ChangeType<string>(int.MaxValue));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new long?()));

            Assert.AreEqual(short.MaxValue.ToString(CultureInfo.InvariantCulture), ConvertUtils.ChangeType<string>(short.MaxValue));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new short?()));

            Assert.AreEqual(byte.MaxValue.ToString(CultureInfo.InvariantCulture), ConvertUtils.ChangeType<string>(byte.MaxValue));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new byte?()));

            Assert.AreEqual(decimal.MaxValue.ToString(CultureInfo.InvariantCulture), ConvertUtils.ChangeType<string>(decimal.MaxValue));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new decimal?()));

            Assert.AreEqual(float.MaxValue.ToString(CultureInfo.InvariantCulture), ConvertUtils.ChangeType<string>(3.40282347E+38F));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new float?()));

            Assert.AreEqual(new DateTime(2009, 11, 20).ToString(CultureInfo.InvariantCulture), ConvertUtils.ChangeType<string>(new DateTime(2009, 11, 20)));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new DateTime?()));

            Guid guid = Guid.NewGuid();
            Assert.AreEqual(guid.ToString(), ConvertUtils.ChangeType<string>(guid));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new Guid?()));

            Assert.AreEqual("Value1", ConvertUtils.ChangeType<string>(TestEnum.Value1));
            Assert.AreEqual("Value2", ConvertUtils.ChangeType<string>(TestEnum.Value2));
            Assert.AreEqual("Value3", ConvertUtils.ChangeType<string>(TestEnum.Value3));
        }

        [Test]
        public void EnsureCanConvertToStringTurkishCulture()
        {
            CultureInfo culture = new CultureInfo("tr-TR");

            bool? nullBool = null;

            Assert.AreEqual(bool.TrueString, ConvertUtils.ChangeType<string>(true, culture));
            Assert.AreEqual(bool.FalseString, ConvertUtils.ChangeType<string>(false, culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(nullBool, culture));

            Assert.AreEqual(int.MaxValue.ToString(culture), ConvertUtils.ChangeType<string>(int.MaxValue, culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new int?(), culture));

            Assert.AreEqual(int.MaxValue.ToString(culture), ConvertUtils.ChangeType<string>(int.MaxValue, culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new long?(), culture));

            Assert.AreEqual(short.MaxValue.ToString(culture), ConvertUtils.ChangeType<string>(short.MaxValue, culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new short?(), culture));

            Assert.AreEqual(byte.MaxValue.ToString(culture), ConvertUtils.ChangeType<string>(byte.MaxValue, culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new byte?(), culture));

            Assert.AreEqual(decimal.MaxValue.ToString(culture), ConvertUtils.ChangeType<string>(decimal.MaxValue, culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new decimal?(), culture));

            Assert.AreEqual(float.MaxValue.ToString(culture), ConvertUtils.ChangeType<string>(3.40282347E+38F, culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new float?(), culture));

            Assert.AreEqual(new DateTime(2009, 11, 20).ToString(culture), ConvertUtils.ChangeType<string>(new DateTime(2009, 11, 20), culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new DateTime?(), culture));

            Guid guid = Guid.NewGuid();
            Assert.AreEqual(guid.ToString(), ConvertUtils.ChangeType<string>(guid, culture));
            Assert.AreEqual(string.Empty, ConvertUtils.ChangeType<string>(new Guid?(), culture));

            Assert.AreEqual("Value1", ConvertUtils.ChangeType<string>(TestEnum.Value1, culture));
            Assert.AreEqual("Value2", ConvertUtils.ChangeType<string>(TestEnum.Value2, culture));
            Assert.AreEqual("Value3", ConvertUtils.ChangeType<string>(TestEnum.Value3, culture));
        }

        [Test]
        public void EnsureCanConvertToOtherTypes()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            Assert.AreEqual((short)byte.MaxValue, ConvertUtils.ChangeType<short>(byte.MaxValue));
            Assert.AreEqual((short?)byte.MaxValue, ConvertUtils.ChangeType<short?>(byte.MaxValue));

            Assert.AreEqual((int)byte.MaxValue, ConvertUtils.ChangeType<int>(byte.MaxValue));
            Assert.AreEqual((int?)byte.MaxValue, ConvertUtils.ChangeType<int?>(byte.MaxValue));
            Assert.AreEqual((int)short.MaxValue, ConvertUtils.ChangeType<int>(short.MaxValue));
            Assert.AreEqual((int?)short.MaxValue, ConvertUtils.ChangeType<int?>(short.MaxValue));

            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(bool.FalseString));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>("no"));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(0));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(string.Empty));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(null));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(string.Empty));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(bool.TrueString));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>("yes"));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(1));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(int.MaxValue));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(long.MaxValue));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(decimal.MaxValue));

            Assert.AreEqual((long)byte.MaxValue, ConvertUtils.ChangeType<long>(byte.MaxValue));
            Assert.AreEqual((long?)byte.MaxValue, ConvertUtils.ChangeType<long?>(byte.MaxValue));
            Assert.AreEqual((long)short.MaxValue, ConvertUtils.ChangeType<long>(short.MaxValue));
            Assert.AreEqual((long?)short.MaxValue, ConvertUtils.ChangeType<long?>(short.MaxValue));
            Assert.AreEqual((long)int.MaxValue, ConvertUtils.ChangeType<long>(int.MaxValue));
            Assert.AreEqual((long?)int.MaxValue, ConvertUtils.ChangeType<long?>(int.MaxValue));

            Assert.AreEqual((float)byte.MaxValue, ConvertUtils.ChangeType<float>(byte.MaxValue));
            Assert.AreEqual((float?)byte.MaxValue, ConvertUtils.ChangeType<float?>(byte.MaxValue));
            Assert.AreEqual((float)short.MaxValue, ConvertUtils.ChangeType<float>(short.MaxValue));
            Assert.AreEqual((float?)short.MaxValue, ConvertUtils.ChangeType<float?>(short.MaxValue));
            Assert.AreEqual((float)int.MaxValue, ConvertUtils.ChangeType<float>(int.MaxValue));
            Assert.AreEqual((float?)int.MaxValue, ConvertUtils.ChangeType<float?>(int.MaxValue));
            Assert.AreEqual((float)long.MaxValue, ConvertUtils.ChangeType<float>(long.MaxValue));
            Assert.AreEqual((float?)long.MaxValue, ConvertUtils.ChangeType<float?>(long.MaxValue));

            Assert.AreEqual((double)byte.MaxValue, ConvertUtils.ChangeType<double>(byte.MaxValue));
            Assert.AreEqual((double?)byte.MaxValue, ConvertUtils.ChangeType<double?>(byte.MaxValue));
            Assert.AreEqual((double)short.MaxValue, ConvertUtils.ChangeType<double>(short.MaxValue));
            Assert.AreEqual((double?)short.MaxValue, ConvertUtils.ChangeType<double?>(short.MaxValue));
            Assert.AreEqual((double)int.MaxValue, ConvertUtils.ChangeType<double>(int.MaxValue));
            Assert.AreEqual((double?)int.MaxValue, ConvertUtils.ChangeType<double?>(int.MaxValue));
            Assert.AreEqual((double)long.MaxValue, ConvertUtils.ChangeType<double>(long.MaxValue));
            Assert.AreEqual((double?)long.MaxValue, ConvertUtils.ChangeType<double?>(long.MaxValue));
            Assert.AreEqual((double)float.MaxValue, ConvertUtils.ChangeType<double>(float.MaxValue));
            Assert.AreEqual((double?)float.MaxValue, ConvertUtils.ChangeType<double?>(float.MaxValue));

            Assert.AreEqual((decimal)byte.MaxValue, ConvertUtils.ChangeType<decimal>(byte.MaxValue));
            Assert.AreEqual((decimal?)byte.MaxValue, ConvertUtils.ChangeType<decimal?>(byte.MaxValue));
            Assert.AreEqual((decimal)short.MaxValue, ConvertUtils.ChangeType<decimal>(short.MaxValue));
            Assert.AreEqual((decimal?)short.MaxValue, ConvertUtils.ChangeType<decimal?>(short.MaxValue));
            Assert.AreEqual((decimal)int.MaxValue, ConvertUtils.ChangeType<decimal>(int.MaxValue));
            Assert.AreEqual((decimal?)int.MaxValue, ConvertUtils.ChangeType<decimal?>(int.MaxValue));
            Assert.AreEqual((decimal)long.MaxValue, ConvertUtils.ChangeType<decimal>(long.MaxValue));
            Assert.AreEqual((decimal?)long.MaxValue, ConvertUtils.ChangeType<decimal?>(long.MaxValue));
        }

        [Test]
        public void EnsureCanConvertToOtherTypesTurkishCulture()
        {
            CultureInfo culture = new CultureInfo("tr-TR");

            Assert.AreEqual((short)byte.MaxValue, ConvertUtils.ChangeType<short>(byte.MaxValue, culture));
            Assert.AreEqual((short?)byte.MaxValue, ConvertUtils.ChangeType<short?>(byte.MaxValue, culture));

            Assert.AreEqual((int)byte.MaxValue, ConvertUtils.ChangeType<int>(byte.MaxValue, culture));
            Assert.AreEqual((int?)byte.MaxValue, ConvertUtils.ChangeType<int?>(byte.MaxValue, culture));
            Assert.AreEqual((int)short.MaxValue, ConvertUtils.ChangeType<int>(short.MaxValue, culture));
            Assert.AreEqual((int?)short.MaxValue, ConvertUtils.ChangeType<int?>(short.MaxValue, culture));

            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(bool.FalseString, culture));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>("no", culture));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(0, culture));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(string.Empty, culture));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(null, culture));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(string.Empty, culture));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(bool.TrueString, culture));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>("yes", culture));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(1, culture));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(int.MaxValue, culture));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(long.MaxValue, culture));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(decimal.MaxValue, culture));

            Assert.AreEqual((long)byte.MaxValue, ConvertUtils.ChangeType<long>(byte.MaxValue, culture));
            Assert.AreEqual((long?)byte.MaxValue, ConvertUtils.ChangeType<long?>(byte.MaxValue, culture));
            Assert.AreEqual((long)short.MaxValue, ConvertUtils.ChangeType<long>(short.MaxValue, culture));
            Assert.AreEqual((long?)short.MaxValue, ConvertUtils.ChangeType<long?>(short.MaxValue, culture));
            Assert.AreEqual((long)int.MaxValue, ConvertUtils.ChangeType<long>(int.MaxValue, culture));
            Assert.AreEqual((long?)int.MaxValue, ConvertUtils.ChangeType<long?>(int.MaxValue, culture));

            Assert.AreEqual((float)byte.MaxValue, ConvertUtils.ChangeType<float>(byte.MaxValue, culture));
            Assert.AreEqual((float?)byte.MaxValue, ConvertUtils.ChangeType<float?>(byte.MaxValue, culture));
            Assert.AreEqual((float)short.MaxValue, ConvertUtils.ChangeType<float>(short.MaxValue, culture));
            Assert.AreEqual((float?)short.MaxValue, ConvertUtils.ChangeType<float?>(short.MaxValue, culture));
            Assert.AreEqual((float)int.MaxValue, ConvertUtils.ChangeType<float>(int.MaxValue, culture));
            Assert.AreEqual((float?)int.MaxValue, ConvertUtils.ChangeType<float?>(int.MaxValue, culture));
            Assert.AreEqual((float)long.MaxValue, ConvertUtils.ChangeType<float>(long.MaxValue, culture));
            Assert.AreEqual((float?)long.MaxValue, ConvertUtils.ChangeType<float?>(long.MaxValue, culture));

            Assert.AreEqual((double)byte.MaxValue, ConvertUtils.ChangeType<double>(byte.MaxValue, culture));
            Assert.AreEqual((double?)byte.MaxValue, ConvertUtils.ChangeType<double?>(byte.MaxValue, culture));
            Assert.AreEqual((double)short.MaxValue, ConvertUtils.ChangeType<double>(short.MaxValue, culture));
            Assert.AreEqual((double?)short.MaxValue, ConvertUtils.ChangeType<double?>(short.MaxValue, culture));
            Assert.AreEqual((double)int.MaxValue, ConvertUtils.ChangeType<double>(int.MaxValue, culture));
            Assert.AreEqual((double?)int.MaxValue, ConvertUtils.ChangeType<double?>(int.MaxValue, culture));
            Assert.AreEqual((double)long.MaxValue, ConvertUtils.ChangeType<double>(long.MaxValue, culture));
            Assert.AreEqual((double?)long.MaxValue, ConvertUtils.ChangeType<double?>(long.MaxValue, culture));
            Assert.AreEqual((double)float.MaxValue, ConvertUtils.ChangeType<double>(float.MaxValue, culture));
            Assert.AreEqual((double?)float.MaxValue, ConvertUtils.ChangeType<double?>(float.MaxValue, culture));

            Assert.AreEqual((decimal)byte.MaxValue, ConvertUtils.ChangeType<decimal>(byte.MaxValue, culture));
            Assert.AreEqual((decimal?)byte.MaxValue, ConvertUtils.ChangeType<decimal?>(byte.MaxValue, culture));
            Assert.AreEqual((decimal)short.MaxValue, ConvertUtils.ChangeType<decimal>(short.MaxValue, culture));
            Assert.AreEqual((decimal?)short.MaxValue, ConvertUtils.ChangeType<decimal?>(short.MaxValue, culture));
            Assert.AreEqual((decimal)int.MaxValue, ConvertUtils.ChangeType<decimal>(int.MaxValue, culture));
            Assert.AreEqual((decimal?)int.MaxValue, ConvertUtils.ChangeType<decimal?>(int.MaxValue, culture));
            Assert.AreEqual((decimal)long.MaxValue, ConvertUtils.ChangeType<decimal>(long.MaxValue, culture));
            Assert.AreEqual((decimal?)long.MaxValue, ConvertUtils.ChangeType<decimal?>(long.MaxValue, culture));
        }

        [Test]
        public void EnsureCanConvertFromStringToOtherTypesWhenDefaultValueIsSetInvariantCulture()
        {
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = invariantCulture;

            EnsureCanConvertFromStringToOtherTypesWhenDefaultValueIsSet(invariantCulture);
        }

        [Test]
        public void EnsureCanConvertFromStringToOtherTypesWhenDefaultValueIsSetTurkishCulture()
        {
            EnsureCanConvertFromStringToOtherTypesWhenDefaultValueIsSet(new CultureInfo("tr-TR"));
        }

        private static void EnsureCanConvertFromStringToOtherTypesWhenDefaultValueIsSet(CultureInfo culture)
        {
            string nullString = null;

            Assert.AreEqual(int.MaxValue, ConvertUtils.ChangeType<int>(nullString, int.MaxValue, culture));
            Assert.AreEqual(int.MaxValue, ConvertUtils.ChangeType<int?>(string.Empty, int.MaxValue, culture));
            Assert.AreEqual(new int?(), ConvertUtils.ChangeType<int?>(nullString, new int?(), culture));

            Assert.AreEqual(long.MaxValue, ConvertUtils.ChangeType<long>(nullString, long.MaxValue, culture));
            Assert.AreEqual(long.MaxValue, ConvertUtils.ChangeType<long?>(string.Empty, long.MaxValue, culture));
            Assert.AreEqual(new long?(), ConvertUtils.ChangeType<long?>(nullString, new long?(), culture));

            Assert.AreEqual(short.MaxValue, ConvertUtils.ChangeType<short>(nullString, short.MaxValue, culture));
            Assert.AreEqual(short.MaxValue, ConvertUtils.ChangeType<short?>(string.Empty, short.MaxValue, culture));
            Assert.AreEqual(new short?(), ConvertUtils.ChangeType<short?>(nullString, new short?(), culture));

            Assert.AreEqual(byte.MaxValue, ConvertUtils.ChangeType<byte>(nullString, byte.MaxValue, culture));
            Assert.AreEqual(byte.MaxValue, ConvertUtils.ChangeType<byte?>(string.Empty, byte.MaxValue, culture));
            Assert.AreEqual(new byte?(), ConvertUtils.ChangeType<byte?>(nullString, new byte?(), culture));

            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(string.Empty, false, culture));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(nullString, false, culture));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(string.Empty, new bool?(), culture));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(nullString, new bool?(), culture));

            Assert.AreEqual(decimal.MaxValue, ConvertUtils.ChangeType<decimal>(nullString, decimal.MaxValue, culture));
            Assert.AreEqual(decimal.MaxValue, ConvertUtils.ChangeType<decimal?>(string.Empty, decimal.MaxValue, culture));
            Assert.AreEqual(new decimal?(), ConvertUtils.ChangeType<decimal?>(nullString, new decimal?(), culture));

            Assert.AreEqual(float.MaxValue, ConvertUtils.ChangeType<float>(nullString, Convert.ToSingle("3.40282347E+38", CultureInfo.InvariantCulture), culture));
            Assert.AreEqual(float.MaxValue, ConvertUtils.ChangeType<float?>(string.Empty, Convert.ToSingle("3.40282347E+38", CultureInfo.InvariantCulture), culture));
            Assert.AreEqual(new float?(), ConvertUtils.ChangeType<float?>(nullString, new float?(), culture));

            Assert.AreEqual(DateTime.MaxValue, ConvertUtils.ChangeType<DateTime>(nullString, Convert.ToDateTime("9999-12-31 23:59:59.9999999", CultureInfo.InvariantCulture), culture));
            Assert.AreEqual(DateTime.MaxValue, ConvertUtils.ChangeType<DateTime?>(string.Empty, Convert.ToDateTime("9999-12-31 23:59:59.9999999", CultureInfo.InvariantCulture), culture));
            Assert.AreEqual(new DateTime?(), ConvertUtils.ChangeType<DateTime?>(nullString, new DateTime?(), culture));

            Guid guid = Guid.NewGuid();
            Assert.AreEqual(guid, ConvertUtils.ChangeType<Guid>(nullString, guid, culture));
            Assert.AreEqual(guid, ConvertUtils.ChangeType<Guid?>(string.Empty, guid, culture));
            Assert.AreEqual(new Guid?(), ConvertUtils.ChangeType<Guid?>(nullString, new Guid?(), culture));

            Assert.AreEqual(TestEnum.Value1, ConvertUtils.ChangeType<TestEnum>(nullString, TestEnum.Value1, culture));
            Assert.AreEqual(TestEnum.Value1, ConvertUtils.ChangeType<TestEnum?>(string.Empty, TestEnum.Value1, culture));
            Assert.AreEqual(new TestEnum?(), ConvertUtils.ChangeType<TestEnum?>(nullString, new TestEnum?(), culture));
        }

        [Test]
        public void EnsureCanConvertToOtherTypesWhenDefaultValueIsSetInvariantCulture()
        {
            CultureInfo invariantCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = invariantCulture;

            EnsureCanConvertToOtherTypesWhenDefaultValueIsSet(invariantCulture);
        }

        [Test]
        public void EnsureCanConvertToOtherTypesWhenDefaultValueIsSetTurkishCulture()
        {
            EnsureCanConvertToOtherTypesWhenDefaultValueIsSet(new CultureInfo("tr-TR"));
        }

        private static void EnsureCanConvertToOtherTypesWhenDefaultValueIsSet(CultureInfo culture)
        {
            Assert.AreEqual((short)byte.MaxValue, ConvertUtils.ChangeType<short>(null, byte.MaxValue, culture));
            Assert.AreEqual((short?)byte.MaxValue, ConvertUtils.ChangeType<short?>(null, byte.MaxValue, culture));

            Assert.AreEqual((int)byte.MaxValue, ConvertUtils.ChangeType<int>(null, byte.MaxValue, culture));
            Assert.AreEqual((int?)byte.MaxValue, ConvertUtils.ChangeType<int?>(null, byte.MaxValue, culture));
            Assert.AreEqual((int)short.MaxValue, ConvertUtils.ChangeType<int>(null, short.MaxValue, culture));
            Assert.AreEqual((int?)short.MaxValue, ConvertUtils.ChangeType<int?>(null, short.MaxValue, culture));

            Assert.AreEqual(false, ConvertUtils.ChangeType<bool>(null, false, culture));
            Assert.AreEqual(false, ConvertUtils.ChangeType<bool?>(null, false, culture));
            Assert.AreEqual(new bool?(), ConvertUtils.ChangeType<bool?>(null, new bool?(), culture));
            Assert.AreEqual(true, ConvertUtils.ChangeType<bool>(null, true, culture));

            Assert.AreEqual((long)byte.MaxValue, ConvertUtils.ChangeType<long>(null, byte.MaxValue, culture));
            Assert.AreEqual((long?)byte.MaxValue, ConvertUtils.ChangeType<long?>(null, byte.MaxValue, culture));
            Assert.AreEqual((long)short.MaxValue, ConvertUtils.ChangeType<long>(null, short.MaxValue, culture));
            Assert.AreEqual((long?)short.MaxValue, ConvertUtils.ChangeType<long?>(null, short.MaxValue, culture));
            Assert.AreEqual((long)int.MaxValue, ConvertUtils.ChangeType<long>(null, int.MaxValue, culture));
            Assert.AreEqual((long?)int.MaxValue, ConvertUtils.ChangeType<long?>(null, int.MaxValue, culture));

            Assert.AreEqual((float)byte.MaxValue, ConvertUtils.ChangeType<float>(null, byte.MaxValue, culture));
            Assert.AreEqual((float?)byte.MaxValue, ConvertUtils.ChangeType<float?>(null, byte.MaxValue, culture));
            Assert.AreEqual((float)short.MaxValue, ConvertUtils.ChangeType<float>(null, short.MaxValue, culture));
            Assert.AreEqual((float?)short.MaxValue, ConvertUtils.ChangeType<float?>(null, short.MaxValue, culture));
            Assert.AreEqual((float)int.MaxValue, ConvertUtils.ChangeType<float>(null, int.MaxValue, culture));
            Assert.AreEqual((float?)int.MaxValue, ConvertUtils.ChangeType<float?>(null, int.MaxValue, culture));
            Assert.AreEqual((float)long.MaxValue, ConvertUtils.ChangeType<float>(null, long.MaxValue, culture));
            Assert.AreEqual((float?)long.MaxValue, ConvertUtils.ChangeType<float?>(null, long.MaxValue, culture));

            Assert.AreEqual((double)byte.MaxValue, ConvertUtils.ChangeType<double>(null, byte.MaxValue, culture));
            Assert.AreEqual((double?)byte.MaxValue, ConvertUtils.ChangeType<double?>(null, byte.MaxValue, culture));
            Assert.AreEqual((double)short.MaxValue, ConvertUtils.ChangeType<double>(null, short.MaxValue, culture));
            Assert.AreEqual((double?)short.MaxValue, ConvertUtils.ChangeType<double?>(null, short.MaxValue, culture));
            Assert.AreEqual((double)int.MaxValue, ConvertUtils.ChangeType<double>(null, int.MaxValue, culture));
            Assert.AreEqual((double?)int.MaxValue, ConvertUtils.ChangeType<double?>(null, int.MaxValue, culture));
            Assert.AreEqual((double)long.MaxValue, ConvertUtils.ChangeType<double>(null, long.MaxValue, culture));
            Assert.AreEqual((double?)long.MaxValue, ConvertUtils.ChangeType<double?>(null, long.MaxValue, culture));
            Assert.AreEqual((double)float.MaxValue, ConvertUtils.ChangeType<double>(null, float.MaxValue, culture));
            Assert.AreEqual((double?)float.MaxValue, ConvertUtils.ChangeType<double?>(null, float.MaxValue, culture));

            Assert.AreEqual((decimal)byte.MaxValue, ConvertUtils.ChangeType<decimal>(null, byte.MaxValue, culture));
            Assert.AreEqual((decimal?)byte.MaxValue, ConvertUtils.ChangeType<decimal?>(null, byte.MaxValue, culture));
            Assert.AreEqual((decimal)short.MaxValue, ConvertUtils.ChangeType<decimal>(null, short.MaxValue, culture));
            Assert.AreEqual((decimal?)short.MaxValue, ConvertUtils.ChangeType<decimal?>(null, short.MaxValue, culture));
            Assert.AreEqual((decimal)int.MaxValue, ConvertUtils.ChangeType<decimal>(null, int.MaxValue, culture));
            Assert.AreEqual((decimal?)int.MaxValue, ConvertUtils.ChangeType<decimal?>(null, int.MaxValue, culture));
            Assert.AreEqual((decimal)long.MaxValue, ConvertUtils.ChangeType<decimal>(null, long.MaxValue, culture));
            Assert.AreEqual((decimal?)long.MaxValue, ConvertUtils.ChangeType<decimal?>(null, long.MaxValue, culture));
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ConvertThrowsArgumentNullExceptionWhenTypeIsNull()
        {
            ConvertUtils.ChangeType(1, null);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ConvertThrowsArgumentNullExceptionWhenCultureIsNull()
        {
            ConvertUtils.ChangeType(1, typeof(int), null);
        }
    }
}
