using System;
using Labo.Common.Utils;
using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class CryptoUtilsFixture
    {
        [Test, Sequential]
        public void EncryptMd5(
            [Values("12345", "")]string text,
            [Values("�|��plL4�h��N{", "��ُ\0��\t���B~")]string expectedText)
        {
            string encryptMd5 = CryptoUtils.EncryptMd5(text);
            Assert.AreEqual(expectedText, encryptMd5);
        }

        [Test]
        public void EncryptMd5ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => CryptoUtils.EncryptMd5(null));
        }

        [Test, Sequential]
        public void EncryptMd5AsHexString(
            [Values("12345", "")]string text,
            [Values("827ccb0eea8a706c4c34a16891f84e7b", "d41d8cd98f00b204e9800998ecf8427e")]string expectedText)
        {
            string encryptMd5 = CryptoUtils.EncryptMd5AsHexString(text);
            Assert.AreEqual(expectedText, encryptMd5);
        }

        [Test]
        public void EncryptMd5AsHexStringThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => CryptoUtils.EncryptMd5AsHexString(null));
        }
    }
}
