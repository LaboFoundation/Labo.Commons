using Labo.Common.Exceptions;
using Labo.Common.Utils;

using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    public enum Enum1
    {
        A,
        B,
        C
    }

    public enum Enum2 : byte
    {
        A = 20,
        B = 30,
        C = 255
    }

    [TestFixture]
    public class EnumUtilsFixture
    {
       [Test]
       public void GetNamesAndValues()
       {
           var dictionary = EnumUtils.GetNamesAndValues<Enum1>();

           Assert.IsTrue(dictionary.ContainsKey("A"));
           Assert.IsTrue(dictionary.ContainsKey("B"));
           Assert.IsTrue(dictionary.ContainsKey("C"));
           
           Assert.AreEqual(0, dictionary["A"]);
           Assert.AreEqual(1, dictionary["B"]);
           Assert.AreEqual(2 ,dictionary["C"]);
       }

       [Test]
       public void GetNamesAndValuesCustomValues()
       {
           var dictionary = EnumUtils.GetNamesAndValues<Enum2>();

           Assert.IsTrue(dictionary.ContainsKey("A"));
           Assert.IsTrue(dictionary.ContainsKey("B"));
           Assert.IsTrue(dictionary.ContainsKey("C"));

           Assert.AreEqual(20, dictionary["A"]);
           Assert.AreEqual(30, dictionary["B"]);
           Assert.AreEqual(255, dictionary["C"]);
       }

       [Test]
       public void Parse()
       {
           Assert.AreEqual(Enum1.A, EnumUtils.Parse<Enum1>("A"));
           Assert.AreEqual(Enum1.B, EnumUtils.Parse<Enum1>("B"));
           Assert.AreEqual(Enum1.C, EnumUtils.Parse<Enum1>("C"));

           Assert.AreEqual(Enum2.A, EnumUtils.Parse<Enum2>("A"));
           Assert.AreEqual(Enum2.B, EnumUtils.Parse<Enum2>("B"));
           Assert.AreEqual(Enum2.C, EnumUtils.Parse<Enum2>("C"));

           Assert.AreEqual(Enum1.A, EnumUtils.Parse<Enum1>("a", true));
           Assert.AreEqual(Enum1.B, EnumUtils.Parse<Enum1>("b", true));
           Assert.AreEqual(Enum1.C, EnumUtils.Parse<Enum1>("c", true));

           Assert.AreEqual(Enum2.A, EnumUtils.Parse<Enum2>("a", true));
           Assert.AreEqual(Enum2.B, EnumUtils.Parse<Enum2>("b", true));
           Assert.AreEqual(Enum2.C, EnumUtils.Parse<Enum2>("c", true));
       }

       [Test]
       [ExpectedException(typeof(CoreLevelException))]
       public void ParseCaseSensitive()
       {
           Assert.AreEqual(Enum1.A, EnumUtils.Parse<Enum1>("a"));
       }

       [Test]
       public void TryParse()
       {
           Enum1 enum1;
           Assert.IsFalse(EnumUtils.TryParse("a", out enum1));
           Assert.IsFalse(EnumUtils.TryParse("b", out enum1));

           Assert.IsTrue(EnumUtils.TryParse("A", out enum1));
           Assert.IsTrue(EnumUtils.TryParse("B", out enum1));

           Assert.IsTrue(EnumUtils.TryParse("a", true, out enum1));
           Assert.IsTrue(EnumUtils.TryParse("b", true, out enum1));
       }
    }
}
