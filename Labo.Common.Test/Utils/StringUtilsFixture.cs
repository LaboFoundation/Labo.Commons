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
            [Values("aa", "", "  ", "a", "c", "S", ".a", "gAA", null, "iİi",   "ıIı",   "iİi",  "ıIı")]string value,
            [Values("",   "", "",   "",  "",  "",  "",   "",    "",   "tr-TR", "tr-TR", "en-US", "en-US")]string culture,
            [Values("Aa", "", "  ", "A", "C", "S", ".a", "GAA", "",   "İİi",   "IIı",   "Iİi",   "IIı")]string expected)
        {
            string result = StringUtils.Capitalize(value, CultureInfo.GetCultureInfo(culture));

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

        [Test, Sequential]
        public void EqualsOrdinalIgnoreCase(
            [Values("abcd", "abcd", "ABCD", "",   "çöşğü", "i",  "ı")]string a,
            [Values("ABCD", "abcd", "ABCD", "",   "ÇÖŞĞÜ", "İ",  "I")]string b,
            [Values(true  ,  true ,  true , true, true,    false, false)]bool expectedResult)

        {
            Assert.AreEqual(expectedResult, StringUtils.EqualsOrdinalIgnoreCase(a, b));
        }

        [Test, Sequential]
        public void IsNumeric(
            [Values("1234567890", "abcdefgh", "123e10", "+-*/_?", "@<>1234", "",    null)]string a,
            [Values(true,          false,      false,    false,    false,    false, false)]bool expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.IsNumeric(a));
        }

        [Test, Sequential]
        public void Count(
            [Values("aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı", "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı", "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı", "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı",
                    "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı", "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı", "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı", "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı",
                    "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı", "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı", "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı", "aaabbbCCCDDDdFFFÇÇÇçİİİiIIIIı",
                    "",                              null)]string @string,
            [Values('a',                             'b',                             'B',                             'C',                             
                    'c',                             'ç',                             'Ç',                             'İ', 
                    'i',                             'İ',                             'I',                             'ı',
                    ' ',                             'Z',                             'z')]char character,
            [Values(3,                               3,                               0,                               3,                                
                    0,                               1,                               3,                               3,
                    1,                               3,                               4,                               1,
                    0,                               0)]int count)
        {
            Assert.AreEqual(count, StringUtils.Count(@string, character));
        }

        [Test, Sequential]
        public void IsAtoZ(
            [Values('a',  'b',   'B',   'C',  'c',  'ç',  'Ç',    'İ',
                    'i',  'İ',   'I',   'ı',  ' ',  'Z',  'z',    '0',
                    '1',  '9',   '_',   '-',  '*',  '@',  '?',    '%')]char character,
            [Values(true, true,  true,  true, true, false, false, false,
                    true, false, true,  false, false, true,  true,  false,
                    false, false,  false, false, false, false, false, false)]bool expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.IsAtoZ(character));
        }

        [Test, Sequential]
        public void Truncate(
            [Values("Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "", "", null, null)]string value,
            [Values(0, 0, 0, 6,
                    6, 6, 25, 26,
                    6, 6, 25, 26,
                    6, 6, 25, 26,
                    6, 6, 25, 26,
                    0, 6, 25, 26,
                    0, 6, 25, 26)]int startIndex,
            [Values(5, 27, 26, 5,
                    21, 20, 1, 1,
                    21, 20, 1, 1,
                    21, 20, 1, 1,
                    21, 20, 1, 1,
                    0, 0, 0, 0,
                    21, 20, 1, 1)]int length,
            [Values("..", "..", "..", "..",
                    "..", "..", "..", "..",
                    "", "", null, null,
                    "..", "..", "..", "..",
                    "", "", null, null,
                    "", "..", null, null,
                    "", "..", null, null)]string postText,
            [Values("..", "..", "..", "..",
                    "..", "..", "..", "..",
                    "..", "..", "..", "..",
                    "", "", null, null,
                    "", "", null, null,
                    "", "..", null, null,
                    "", "..", null, null)]string preText,
            [Values("Lorem..", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet..", "..ipsum..", 
                    "..ipsum dolor sit amet.", "..ipsum dolor sit amet..", "..t..", "...",
                    "..ipsum dolor sit amet.", "..ipsum dolor sit amet", "..t", "...",
                    "ipsum dolor sit amet.", "ipsum dolor sit amet..", "t..", ".",
                    "ipsum dolor sit amet.", "ipsum dolor sit amet", "t", ".",
                    "", "", "", "",
                    "", "", "", "")]string expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.Truncate(value, startIndex, length, postText, preText));                
        }

        [Test, Sequential]
        public void Truncate(
            [Values("Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet.",
                    "", "", null)]string value,
            [Values(5, 27, 26,
                    21, 20, 27, 26,
                    27, 26, 27, 26,
                    27, 26, 27, 26,
                    27, 26, 27, 26,
                    0, 0, 0,
                    21, 20, 1)]int length,
            [Values("..", "..", "..",
                    "..", "..", "..", "..",
                    "", "", null, null,
                    "..", "..", "..", "..",
                    "", "", null, null,
                    "", "..", null,
                    "", "..", null)]string postText,
            [Values("..", "..", "..",
                    "..", "..", "..", "..",
                    "..", "..", "..", "..",
                    "", "", null, null,
                    "", "", null, null,
                    "", "..", null,
                    "", "..", null)]string preText,
            [Values("Lorem..", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet..",
                    "Lorem ipsum dolor sit..", "Lorem ipsum dolor si..", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet..",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet..", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet..",
                    "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet", "Lorem ipsum dolor sit amet.", "Lorem ipsum dolor sit amet",
                    "", "", "",
                    "", "", "", "")]string expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.Truncate(value, length, postText, preText));
        }

        [Test]
        public void TruncateThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => StringUtils.Truncate("", -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => StringUtils.Truncate("", -1, 0));
        }

        public class Item
        {
            public int Value { get; set; }
        }

        private readonly object[] JoinTestCases = new object[]
            {
                new object[] { new []{ "1", "2", "3" }, ";", null, "1;2;3" },
                new object[] { new []{ "1", "2", "3" }, ";", new Func<string, string>(x => x + x), "11;22;33" },
                new object[] { new []{ "1", "2", "3" }, string.Empty, null, "123" },
                new object[] { new []{ "1", "2", "3" }, null, null, "123" },
                new object[] { new string[]{ null, null, null }, null, null, string.Empty },
                new object[] { new []{ string.Empty, null, null }, null, null, string.Empty },
                new object[] { new string[0], null, null, string.Empty },
                new object[] { new string[0], ";", null, string.Empty },
                new object[] { new string[0], ";", new Func<string, string>(x => x + x), string.Empty }
            };

        private readonly object[] JoinToStringTestCases = new object[]
            {
                new object[] { new []{ new Item{ Value = 1}, new Item{ Value = 2}, new Item{ Value = 3} }, new Func<Item, string>(x => x.Value.ToStringInvariant()), ";", "1;2;3" },
                new object[] { new []{ new Item{ Value = 1}, new Item{ Value = 2}, new Item{ Value = 3} }, new Func<Item, string>(x => x.Value.ToStringInvariant()), string.Empty, "123" },
                new object[] { new []{ new Item{ Value = 1}, new Item{ Value = 2}, new Item{ Value = 3} }, new Func<Item, string>(x => x.Value.ToStringInvariant()), null, "123" },
                new object[] { new Item[]{ null, null, null }, new Func<Item, string>(Convert.ToString), null, string.Empty },
                new object[] { new Item[0], new Func<Item, string>(x => x.Value.ToStringInvariant()), null, string.Empty },
                new object[] { new Item[0], new Func<Item, string>(x => x.Value.ToStringInvariant()), ";", string.Empty },
                new object[] { new Item[0], new Func<Item, string>(x => x.Value.ToStringInvariant()), ";", string.Empty }
            };

        [Test, Sequential]
        [TestCaseSource("JoinTestCases")]
        public void Join(string[] strings, string separator, Func<string, string> func, string expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.Join(strings, separator, func));
        }

        [Test]
        public void JoinThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StringUtils.Join(null));
        }

        [Test, Sequential]
        [TestCaseSource("JoinToStringTestCases")]
        public void JoinToString(Item[] items, Func<Item, string> func, string separator, string expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.JoinToString(items, func, separator));
        }

        [Test]
        public void JoinToStringThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StringUtils.JoinToString<Item>(null, x => x.ToStringInvariant()));
            Assert.Throws<ArgumentNullException>(() => StringUtils.JoinToString<Item>(new Item[0], null));
        }

        [Test, Sequential]
        public void Contains(
            [Values("abc", "abc", "abc", "abc")]string target,
            [Values("ab", "ABC", "ABC", "def")]string value,
            [Values(StringComparison.InvariantCulture, 
                    StringComparison.InvariantCulture, 
                    StringComparison.InvariantCultureIgnoreCase,
                    StringComparison.InvariantCulture)]StringComparison stringComparison,
            [Values(true, false, true, false)]bool expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.Contains(target, value, stringComparison));
        }

        [Test, Sequential]
        public void Contains(
            [Values("abc", "abc", "abc", "abc")]string target,
            [Values("ab", "ABC", "ABC", "bc")]string value,
            [Values(0, 0, 0, 2)]int startIndex,
            [Values(StringComparison.InvariantCulture, StringComparison.InvariantCulture, StringComparison.InvariantCultureIgnoreCase)]StringComparison stringComparison,
            [Values(true, false, true, false)]bool expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.Contains(target, value, startIndex, stringComparison));
        }

        [Test, Sequential]
        public void Contains(
            [Values("abc", "abc", "abc", "abc")]string target,
            [Values("ab", "ABC", "ABC", "def")]string value,
            [Values("", "", "", "")]string culture,
            [Values(true, false, false, false)]bool expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.Contains(target, value, CultureInfo.GetCultureInfo(culture)));
        }

        [Test, Sequential]
        public void Contains(
            [Values("abc", "abc", "abc", "abc")]string target,
            [Values("ab", "ABC", "ABC", "bc")]string value,
            [Values(0, 0, 0, 2)]int startIndex,
            [Values("", "", "", "")]string culture,
            [Values(true, false, false, false)]bool expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.Contains(target, value, startIndex, CultureInfo.GetCultureInfo(culture)));
        }

        [Test, Sequential]
        public void Reverse(
            [Values("a", "bab", "abcd", "", " ", null, "  ")]string text,
            [Values("a", "bab", "dcba", "", " ", null, "  ")]string expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.Reverse(text));
        }

        [Test, Sequential]
        public void ToTitleCase(
            [Values("lorem ipsum dolor sit amet", "a", "bab", "", " ", "  ")]string text,
            [Values("",                           "",   "",   "", "",  "")]string culture,
            [Values("Lorem Ipsum Dolor Sit Amet", "A", "Bab", "", " ", "  ")]string expectedResult)
        {
            string titleCased = StringUtils.ToTitleCase(text, CultureInfo.GetCultureInfo(culture));
            Assert.AreEqual(expectedResult, titleCased);
        }

        [Test, Sequential]
        public void PadLeft(
            [Values("123", "123", "123", "123", "", "")]string text,
            [Values('0'  , '0'  , '0',   '0',  '0', '0')]char character,
            [Values(5, 3, 4, 2, 5, 0)]int length,
            [Values("00123", "123", "0123", "123", "00000", "")]string expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.PadLeft(text, character, length));
        }

        [Test]
        public void PadLeftThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StringUtils.PadLeft(null, '0', 10));
        }

        [Test, Sequential]
        public void PadRight(
            [Values("123", "123", "123", "123", "", "")]string text,
            [Values('0', '0', '0', '0', '0', '0')]char character,
            [Values(5, 3, 4, 2, 5, 0)]int length,
            [Values("12300", "123", "1230", "123", "00000", "")]string expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.PadRight(text, character, length));
        }

        [Test]
        public void PadRightThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StringUtils.PadRight(null, '0', 10));
        }

        [Test, Sequential]
        public void ToTitleCaseThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StringUtils.ToTitleCase((string)null));
            Assert.Throws<ArgumentNullException>(() => StringUtils.ToTitleCase((string[])null));
        }

        [Test, Sequential]
        public void Enquote(
            [Values(
                "'\"Hello!\"'",
                "\"'Hello!'\"",
                "\"www.google.com\\translate\"",
                "\"'Hello!\b\f\r\t\n'\"",
                null,
                ""
                )]string text,
            [Values(
                "'\\\"Hello!\\\"'",
                "\\\"'Hello!'\\\"",
                "\\\"www.google.com\\\\translate\\\"",
                "\\\"'Hello!\\b\\f\\r\\t\\n'\\\"",
                "",
                ""
                )]string expectedResult)
        {
            string enquotedText = StringUtils.Enquote(text);
            Assert.AreEqual(expectedResult, enquotedText);
        }

        [Test, Sequential]
        public void EncodeJsString(
            [Values(
                "var s = '\"Hello!\"';",
                "var s = \"'Hello!'\";",
                "var s = \"www.google.com\\translate\";",
                "var s = \"'Hello!\b\f\r\t\n'\";",
                "var s = \"ğüşçö\"",
                null,
                ""
                )]string javaScriptText,
            [Values(
                "var s = \\'\\\"Hello!\\\"\\';",
                "var s = \\\"\\'Hello!\\'\\\";",
                "var s = \\\"www.google.com\\\\translate\\\";",
                "var s = \\\"\\'Hello!\\b\\f\\r\\t\\n\\'\\\";",
                "var s = \\\"\\u011F\\u00FC\\u015F\\u00E7\\u00F6\\\"",
                "",
                ""
                )]string expectedResult)
        {
            string encodedJsString = StringUtils.EncodeJsString(javaScriptText);
            Assert.AreEqual(expectedResult, encodedJsString);
        }

        [Test, Sequential]
        public void ReplaceiTurkishCharacters(
            [Values(
                "abcdğüşıöçĞÜŞİÖÇ",
                null,
                ""
                )]string text,
            [Values(
                "abcdgusiocGUSIOC",
                null,
                ""
                )]string expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.ReplaceTurkishCharacters(text));
        }

        [Test, Sequential]
        public void StripHtmlTags(
            [Values("<text>xxx</text>", null, "", "<br/>", "<b>name</b>", "name", "a>b", "<a><b>")]string text,
            [Values( "xxx", null, "", "", "name", "name", "a>b", "")]string expectedResult)
        {
            Assert.AreEqual(expectedResult, StringUtils.StripHtmlTags(text));
        }
    }
}
