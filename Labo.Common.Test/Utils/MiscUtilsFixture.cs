using System.Linq;
using Labo.Common.Utils;
using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class MiscUtilsFixture
    {
        [Test]
        public void GenerateRandomString()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            char[] characterSet = characters.ToCharArray();
            string generatedRandomString = MiscUtils.GenerateRandomString(5, characters);

            Assert.IsTrue(generatedRandomString.ToCharArray().All(characterSet.Contains));
        }

        [Test]
        public void LongToBaseString()
        {
            char[] chars = new[]
                               {
                                   'C', 'J', 'A', '4', 'D', '5', '6', '7', 'H', '9', '0', '1', 'B', '2', 'E', 'F', 'G',
                                   '3', 'I', '8', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
                               };
            for (int i = 0; i < 10000; i++)
            {
                Assert.AreEqual(i, MiscUtils.BaseStringToLong(MiscUtils.LongToBaseString(i, chars), chars));
            }
        }
    }
}
