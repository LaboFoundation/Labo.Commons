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
            Assert.AreEqual("Lorem ipsum dolor sit amet.", AssemblyUtils.GetEmbeddedResourceString(Assembly.GetExecutingAssembly(), "Labo.Common.Tests._TestAssembly.EmbeddedResource.txt"));
        }

        [Test]
        public void GetEmbededResourceStringThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => AssemblyUtils.GetEmbeddedResourceString(null, "", EncodingHelper.CurrentCultureEncoding));
            Assert.Throws<ArgumentNullException>(() => AssemblyUtils.GetEmbeddedResourceString(Assembly.GetExecutingAssembly(), null, EncodingHelper.CurrentCultureEncoding));
            Assert.Throws<ArgumentNullException>(() => AssemblyUtils.GetEmbeddedResourceString(Assembly.GetExecutingAssembly(), "", null));
            Assert.Throws<AssemblyUtilsException>(() => AssemblyUtils.GetEmbeddedResourceString(Assembly.GetExecutingAssembly(), "Labo.Common.Tests.xxx.txt", EncodingHelper.CurrentCultureEncoding));
        }

        [Test]
        public void GetEmbededResourceBinary()
        {
            Assert.AreEqual("ï»¿Lorem ipsum dolor sit amet.", EncodingHelper.CurrentCultureEncoding.GetString(AssemblyUtils.GetEmbeddedResourceBinary(Assembly.GetExecutingAssembly(), "Labo.Common.Tests._TestAssembly.EmbeddedResource.txt")));
        }

        [Test]
        public void GetEmbededResourceBinaryThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => AssemblyUtils.GetEmbeddedResourceBinary(null, ""));
            Assert.Throws<ArgumentNullException>(() => AssemblyUtils.GetEmbeddedResourceBinary(Assembly.GetExecutingAssembly(), null));
        }
    }
}
