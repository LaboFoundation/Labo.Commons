using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
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
        /// Formats the specified string.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string Format(string @string, params object[] args)
        {
            return String.Format(CultureInfo.CurrentCulture, @string, args);
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
            if(String.IsNullOrEmpty(target))
            {
                return String.Empty;
            }

            return String.Join(BR_STRING, target.Split(s_NewLineStrings, StringSplitOptions.None));
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
        /// <returns></returns>
        public static string Truncate(string @string, int length, string postText)
        {
            return Truncate(@string, 0, length, postText);
        }

        /// <summary>
        /// Truncates the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <param name="postText">The post text.</param>
        /// <returns></returns>
        public static string Truncate(string target, int startIndex, int length, string postText)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            if (target == null)
            {
                return string.Empty;
            }

            if (startIndex < 0 || startIndex > target.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            if (length == 0)
            {
                return String.Empty;
            }

            int truncateLength = length + startIndex > target.Length ? target.Length - startIndex : length;
            if (startIndex == 0 && target.Length == truncateLength)
            {
                postText = String.Empty;
            }

            return String.Concat(target.Substring(startIndex, truncateLength), postText);
        }

        /// <summary>
        /// Joins the specified strings.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static string Join(IList<string> strings, string separator, Func<string, string> func = null)
        {
            return Join(strings.ToArray(), separator, func);
        }

        /// <summary>
        /// Joins the specified strings.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        public static string Join(string[] strings, string separator, Func<string, string> func = null)
        {
            if (strings == null) throw new ArgumentNullException("strings");
            if (separator == null) throw new ArgumentNullException("separator");

            int length = strings.Length;
            if (length == 0)
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                string s = strings[i];
                stringBuilder.Append(func != null ? func(s) : s);
                stringBuilder.Append(separator);
            }
            return stringBuilder.ToString(0, stringBuilder.Length - separator.Length);
        }

        /// <summary>
        /// Capitalizes the specified string.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <returns></returns>
        public static string Capitalize(string @string)
        {
            return Capitalize(@string, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Capitalizes the specified string.
        /// </summary>
        /// <param name="string">The string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string Capitalize(string @string, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(@string))
            {
                return String.Empty;
            }

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
        /// MD5Hash's a string. 
        /// </summary>
        public static string ToMd5Hash(string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(value.Trim());
                byte[] hash = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Converts the first character of each word to Uppercase. Example: "the lazy dog" returns "The Lazy Dog"
        /// </summary>
        /// <param name="text">The text to convert to sentence case</param>
        public static string ToTitleCase(string text)
        {
            if (text == null) throw new ArgumentNullException("text");

            return ToTitleCase(text.Split(' '));
        }

        /// <summary>
        /// Converts the first character of each word to Uppercase. Example: "the lazy dog" returns "The Lazy Dog"
        /// </summary>
        public static string ToTitleCase(string[] words)
        {
            if (words == null) throw new ArgumentNullException("words");

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = Char.ToUpper(words[i][0], CultureInfo.CurrentCulture) + words[i].Substring(1);
            }

            return String.Join(" ", words);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public static string ToLowerCamelCase(string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return value.Substring(0, 1).ToLower(CultureInfo.CurrentCulture) + value.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToLowerCamelCase(string[] values)
        {
            return ToLowerCamelCase(ToCamelCase(values));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToCamelCase(string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return value.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture) + value.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToCamelCase(string[] values, string separator)
        {
            if (values == null) throw new ArgumentNullException("values");

            string temp = String.Empty;
            foreach (string s in values)
            {
                temp += separator;
                temp += ToCamelCase(s);
            }
            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToCamelCase(string[] values)
        {
            return ToCamelCase(values, String.Empty);
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
        /// Pad the right side of a string with a '0' if a single character.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string PadRight(string src)
        {
            return PadRight(src, '0', 2);
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
        /// Accepts a string like "ArrowRotateClockwise" and returns "arrow_rotate_clockwise.png".
        /// </summary>
        /// <param name="name"></param>
        /// <param name="separator"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string ToCharacterSeparatedFileName(string name, char separator, string extension)
        {
            MatchCollection match = Regex.Matches(name, @"([A-Z]+)[a-z]*|\d{1,}[a-z]{0,}");

            string temp = String.Empty;

            for (int i = 0; i < match.Count; i++)
            {
                if (i != 0)
                {
                    temp += separator;
                }
                temp += match[i].ToString().ToLower(CultureInfo.CurrentCulture);
            }

            string format = (String.IsNullOrEmpty(extension)) ? "{0}{1}" : "{0}.{1}";

            return String.Format(CultureInfo.CurrentCulture, format, temp, extension);
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

            //sb.Append('"');
            for (i = 0; i < len; i += 1)
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
                        //t = "000" + Integer.toHexString(c); 
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
            //sb.Append('"');
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
            foreach (char c in s)
            {
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
                        int i = (int)c;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EnsureSemiColon(string text)
        {
            return (String.IsNullOrEmpty(text) || text.EndsWith(";", StringComparison.OrdinalIgnoreCase)) ? text : String.Concat(text, ";");
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

        private static readonly Dictionary<string, string> s_TurkishCharacteMap =
            new Dictionary<string, string>
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
            foreach (KeyValuePair<string, string> keyValuePair in s_TurkishCharacteMap)
            {
                text = text.Replace(keyValuePair.Key, keyValuePair.Value);
            }
            return text;
        }
    }
}