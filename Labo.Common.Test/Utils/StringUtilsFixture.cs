using System;
using System.Globalization;

using Labo.Common.Utils;

using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class StringUtilsFixture
    {
        [Test, Sequential]
        public void NewLineToBr(
            [Values("aa\r\r\n", "", "  ", "a", "\r", "\r\n", "\n\r", "\n\r\r", null)]string value,
            [Values("aa<br /><br />", "", "  ", "a", "<br />", "<br />", "<br />", "<br /><br />", "")]string expected)
        {
            string result = StringUtils.ConvertNewLineToBr(value);

            Assert.AreEqual(expected, result);
        }

        [Test, Sequential]
        public void Capitalize(
            [Values("aa", "", "  ", "a", "c", "S", ".a", "gAA", null)]string value,
            [Values("Aa", "", "  ", "A", "C", "S", ".a", "GAA", "")]string expected)
        {
            string result = StringUtils.Capitalize(value);

            Assert.AreEqual(expected, result);
        }

        [Test, Sequential]
        public void CapitalizeTr(
            [Values("aa", "", "  ", "a", "ç", "Ş", ".a", "ğAA", null)]string value,
            [Values("Aa", "", "  ", "A", "Ç", "Ş", ".a", "ĞAA", "")]string expected)
        {
            string result = StringUtils.Capitalize(value, new CultureInfo("tr-TR"));

            Assert.AreEqual(expected, result);
        }

        [Test, Sequential]
        public void Left(
            [Values("123456789", "123456789", "123456789", "123456789", "")]string value,
            [Values(3, 0, 9, 10, 1)]int length,
            [Values("123", "", "123456789", "123456789", "")]string expected)
        {
            string result = StringUtils.Left(value, length);

            Assert.AreEqual(expected, result);
        }

        [Test, Sequential]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LeftOutOfRange(
            [Values("123456789","")]string value,
            [Values(-1, -1)]int length,
            [Values(null, null)]string expected)
        {
            string result = StringUtils.Left(value, length);

            Assert.AreEqual(expected, result);
        }

        [Test, Sequential]
        public void Right(
            [Values("123456789", "123456789", "123456789", "123456789", "")]string value,
            [Values(3, 0, 9, 10, 1)]int length,
            [Values("789", "", "123456789", "123456789", "")]string expected)
        {
            string result = StringUtils.Right(value, length);

            Assert.AreEqual(expected, result);
        }

        [Test, Sequential]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RightOutOfRange(
            [Values("123456789", "")]string value,
            [Values(-1, -1)]int length,
            [Values(null, null)]string expected)
        {
            string result = StringUtils.Right(value, length);

            Assert.AreEqual(expected, result);
        }
    }
}
