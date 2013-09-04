using System;
using System.IO;
using System.Text;
using Labo.Common.Utils;
using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class IOUtilsFixture
    {
        [Test]
        public void ReadAllText()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_TestFiles\\TestFile1.txt");
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                Assert.AreEqual("Lorem ipsum dolor sit amet.", IOUtils.ReadAllText(fileStream, Encoding.UTF8));                
            }
        }
    }
}
