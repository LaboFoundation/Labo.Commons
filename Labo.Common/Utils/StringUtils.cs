using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BR_STRING = "<br />";
        private static readonly string[] s_NewLineStrings = new[] { "\r\n", "\n\r", "\n", "\r" };

        /// <summary>
        ///  Determines whether two specified <see cref="T:System.String"/> objects have the same value using ordinal ignorecase comparison
        /// </summary>
        /// <param name="a">The first string value.</param>
        /// <param name="b">The second string value.</param>
        /// <returns></returns>
        public static bool EqualsOrdinalIgnoreCase(string a, string b)
        {
            return String.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified string is numeric.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(string @string)
        {
            if (String.IsNullOrEmpty(@string))
            {
                return false;
            }
            char[] chars = @string.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (!Char.IsNumber(chars[i]))
                {
                    return false;
                }
            }
            return true;
        } 

        /// <summary>
        /// Converts the new line to br.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static string ConvertNewLineToBr(string target)
        {
            return ReplaceNewLine(target, BR_STRING);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceNewLine(string target, string newValue)
        {
            if (String.IsNullOrEmpty(target))
            {
                return String.Empty;
            }

            return String.Join(newValue, target.Split(s_NewLineStrings, StringSplitOptions.None));
        }

        /// <summary>
        /// Counts the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="character">The character.</param>
        /// <returns></returns>
        public static int Count(string target, char character)
        {
            if (String.IsNullOrEmpty(target))
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < target.Length; i++)
            {
                char c = target[i];
                if (c.CompareTo(character) == 0)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Determines whether [is A to Z] [the specified @char].
        /// </summary>
        /// <param name="char">The @char.</param>
        /// <returns>
        ///   <c>true</c> if [is A to Z] [the specified @char]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAtoZ(char @char)
        {
            return (@char >= 'a' && @char <= 'z') || (@char >= 'A' && @char <= 'Z');
        }

        /// <summary>
        /// Truncates the specified @string.
        /// </summary>
        /// <param name="string">The @string.</param>
        /// <param name="length">The length.</param>
        /// <param name="postText">The post text.</param>
        /// <param name="preText">The pre text</param>
        /// <returns></returns>
        public static string Truncate(string @string, int length, string postText = null, string preText = null)
        {
            return Truncate(@string, 0, length, postText, preText);
        }

        /// <summary>
        /// Truncates the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <param name="postText">The post text.</param>
        /// <param name="preText">The pre text</param>
        /// <returns></returns>
        public static string Truncate(string target, int startIndex, int length, string postText = null, string preText = null)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length");
            if (target == null) return string.Empty;
            if (startIndex < 0) throw new ArgumentOutOfRangeException("startIndex");

            if (length == 0 || startIndex > target.Length)
            {
                return String.Empty;
            }

            bool truncateToTargetsLastChar = length + startIndex >= target.Length;
            int truncateLength = truncateToTargetsLastChar ? target.Length - startIndex : length;

            if (truncateToTargetsLastChar)
            {
                postText = String.Empty;
            }
            if (startIndex > 0 && preText != null)
            {
                return String.Concat(preText, target.Substring(startIndex, truncateLength), postText);                
            }

            return String.Concat(target.Substring(startIndex, truncateLength), postText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <param name="separator"></param>
        /// <typeparam name="TItem"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string JoinToString<TItem>(IEnumerable<TItem> items, Func<TItem, string> func, string separator = null)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (func == null) throw new ArgumentNullException("func");

            StringBuilder stringBuilder = new StringBuilder();
            foreach (TItem item in items)
            {
                stringBuilder.Append(func(item));
                stringBuilder.Append(separator);
            }
            return GetJoinedStringResult(stringBuilder, separator);
        }

        /// <summary>
        /// Joins the specified strings.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static string Join(IEnumerable<string> strings, string separator = null, Func<string, string> func = null)
        {
            if (strings == null) throw new ArgumentNullException("strings");

            StringBuilder stringBuilder = new StringBuilder();
            foreach (string s in strings)
            {
                stringBuilder.Append(func != null ? func(s) : s);
                stringBuilder.Append(separator);
            }
            return GetJoinedStringResult(stringBuilder, separator);
        }

        private static string GetJoinedStringResult(StringBuilder stringBuilder, string separator)
        {
            return separator == null
                       ? stringBuilder.ToString()
                       : stringBuilder.ToString(0, Math.Max(0, stringBuilder.Length - separator.Length));
        }

        /// <summary>
        /// Capitalizes the specified string.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string Capitalize(string @string, CultureInfo culture = null)
        {
            if (String.IsNullOrEmpty(@string))
            {
                return String.Empty;
            }

            culture = CultureUtils.GetCurrentCultureIfNull(culture);

            if (@string.Length == 1)
            {
                return @string.ToUpper(culture);
            }

            return @string.Substring(0, 1).ToUpper(culture) + @string.Substring(1);
        }

        /// <summary>
        /// Determines whether the [value] [contains] in [the specified target].
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>
        ///   <c>true</c> if the [value] [contains] in [the specified target]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(string target, string value, StringComparison comparisonType)
        {
            return target != null && value != null && target.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static bool Contains(string target, string value, int startIndex, StringComparison comparisonType)
        {
            return target != null && value != null && target.IndexOf(value, startIndex, comparisonType) >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static bool Contains(string target, string value, CultureInfo culture = null)
        {
            culture = CultureUtils.GetCurrentCultureIfNull(culture);

            return target != null && value != null && culture.CompareInfo.IndexOf(target, value) >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="startIndex"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static bool Contains(string target, string value, int startIndex, CultureInfo culture = null)
        {
            culture = CultureUtils.GetCurrentCultureIfNull(culture);

            return target != null && value != null && culture.CompareInfo.IndexOf(target, value, startIndex) >= 0;
        }

        /// <summary>
        /// Lefts the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string Left(string target, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            if (String.IsNullOrEmpty(target))
            {
                return target;
            }

            return target.Substring(0, length > target.Length ? target.Length : length);
        }

        /// <summary>
        /// Receives string and returns the string with its letters reversed.
        /// </summary>
        public static string Reverse(string s)
        {
            if(String.IsNullOrWhiteSpace(s))
            {
                return s;
            }
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        /// <summary>
        /// Rights the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string Right(string target, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            if (String.IsNullOrEmpty(target))
            {
                return target;
            }

            return target.Substring(length > target.Length ? 0 : target.Length - length);
        }

        /// <summary>
        /// Converts the first character of each word to Uppercase. Example: "the lazy dog" returns "The Lazy Dog"
        /// </summary>
        /// <param name="text">The text to convert to sentence case</param>
        /// <param name="culture"> </param>
        public static string ToTitleCase(string text, CultureInfo culture = null)
        {
            if (text == null) throw new ArgumentNullException("text");

            return ToTitleCase(text.Split(' '), culture);
        }

        /// <summary>
        /// Converts the first character of each word to Uppercase. Example: "the lazy dog" returns "The Lazy Dog"
        /// </summary>
        public static string ToTitleCase(string[] words, CultureInfo culture = null)
        {
            if (words == null) throw new ArgumentNullException("words");

            culture = CultureUtils.GetCurrentCultureIfNull(culture);

            string[] titleCasedWords = new string[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if (string.IsNullOrEmpty(word))
                {
                    titleCasedWords[i] = word;
                }
                else
                {
                    titleCasedWords[i] = Char.ToUpper(word[0], culture) + word.Substring(1);                    
                }
            }

            return String.Join(" ", titleCasedWords);
        }

        /// <summary>
        ///  Pad the left side of a string with characters to make the total length.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="c"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string PadLeft(string src, char c, int totalLength)
        {
            if (src == null) throw new ArgumentNullException("src");

            if (totalLength < src.Length)
            {
                return src;
            }
            return new string(c, totalLength - src.Length) + src;
        }

        /// <summary>
        /// Pad the right side of a string with characters to make the total length.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="c"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string PadRight(string src, char c, int totalLength)
        {
            if (src == null) throw new ArgumentNullException("src");

            if (totalLength < src.Length)
            {
                return src;
            }
            return src + new string(c, totalLength - src.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Enquote(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);

            for (i = 0; i < len; i ++)
            {
                char c = s[i];
                if ((c == '\\') || (c == '"') || (c == '>'))
                {
                    sb.Append('\\');
                    sb.Append(c);
                }
                else if (c == '\b')
                    sb.Append("\\b");
                else if (c == '\t')
                    sb.Append("\\t");
                else if (c == '\n')
                    sb.Append("\\n");
                else if (c == '\f')
                    sb.Append("\\f");
                else if (c == '\r')
                    sb.Append("\\r");
                else
                {
                    if (c < ' ')
                    {
                        string tmp = new string(c, 1);
                        string t = "000" + Int32.Parse(tmp, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                        sb.Append("\\u" + t.Substring(t.Length - 4));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        /// is essentially a JSON string.
        /// 
        /// The string returned includes outer quotes 
        /// Example Output: "Hello \"Rick\"!\r\nRock on"
        /// 
        /// http://www.west-wind.com/weblog/posts/2007/Jul/14/Embedding-JavaScript-Strings-from-an-ASPNET-Page
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeJsString(string s)
        {
            if (s == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < s.Length; j++)
            {
                char c = s[j];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\'':
                        sb.Append("\\\'");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;

                    default:
                        int i = (int) c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }

            return sb.ToString();
        }

        private static readonly Regex s_StripHtmlRegex = new Regex("<(.|\n)*?>", RegexOptions.Compiled);
        
        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string StripHtmlTags(string source)
        {
            if(String.IsNullOrEmpty(source))
            {
                return source;
            }

            return s_StripHtmlRegex.Replace(source, String.Empty);
        }

        private static readonly SortedList<string, string> s_TurkishCharacteMap =
            new SortedList<string, string>
                {
                    {"ğ", "g"},
                    {"ü", "u"},
                    {"ı", "i"},
                    {"ç", "c"},
                    {"ş", "s"},
                    {"ö", "o"},
                    {"Ğ", "G"},
                    {"Ü", "U"},
                    {"İ", "I"},
                    {"Ç", "C"},
                    {"Ş", "S"},
                    {"Ö", "O"},
                };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceTurkishCharacters(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            foreach (KeyValuePair<string, string> keyValuePair in s_TurkishCharacteMap)
            {
                text = text.Replace(keyValuePair.Key, keyValuePair.Value);
            }
            return text;
        }
    }
}