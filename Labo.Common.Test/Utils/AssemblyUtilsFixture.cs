using System;
using System.IO;
using System.Reflection;

using Labo.Common.Culture;
using Labo.Common.Utils;
using Labo.Common.Utils.Exceptions;

using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class AssemblyUtilsFixture
    {
        [Test, Ignore]
        public void GetAssemblyTime()
        {
            string assemblyFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\_TestAssembly\\Labo.Common.Tests.dll");
            DateTime assemblyTime = AssemblyUtils.GetAssemblyTime(Assembly.LoadFrom(assemblyFile));
            Assert.AreEqual(new DateTime(635135048686377802), assemblyTime);
        }

        [Test]
        public void GetEmbededResourceString()
        {
            Assert.AreEqual("Lorem ipsum dolor sit amet.", AssemblyUtils.GetEmbededResourceString(Assembly.GetExecutingAssembly(), "Labo.Common.Tests._TestAssembly.EmbeddedResource.txt"));
        }

        [Test]
        public void GetEmbededResourceStringThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => AssemblyUtils.GetEmbededResourceString(null, "", EncodingHelper.CurrentCultureEncoding));
            Assert.Throws<ArgumentNullException>(() => AssemblyUtils.GetEmbededResourceString(Assembly.GetExecutingAssembly(), null, EncodingHelper.CurrentCultureEncoding));
            Assert.Throws<ArgumentNullException>(() => AssemblyUtils.GetEmbededResourceString(Assembly.GetExecutingAssembly(), "", null));
            Assert.Throws<AssemblyUtilsException>(() => AssemblyUtils.GetEmbededResourceString(Assembly.GetExecutingAssembly(), "Labo.Common.Tests.xxx.txt", EncodingHelper.CurrentCultureEncoding));
        }

        [Test]
        public void GetEmbededResourceBinary()
        {
            Assert.AreEqual("ï»¿Lorem ipsum dolor sit amet.", EncodingHelper.CurrentCultureEncoding.GetString(AssemblyUtils.GetEmbededResourceBinary(Assembly.GetExecutingAssembly(), "Labo.Common.Tests._TestAssembly.EmbeddedResource.txt")));
        }

        [Test]
        public void GetEmbededResourceBinaryThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => AssemblyUtils.GetEmbededResourceBinary(null, ""));
            Assert.Throws<ArgumentNullException>(() => AssemblyUtils.GetEmbededResourceBinary(Assembly.GetExecutingAssembly(), null));
        }
    }
}
