// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MiscUtils.cs" company="Labo">
//   The MIT License (MIT)
//   
//   Copyright (c) 2013 Bora Akgun
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to
//   use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//   the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the MiscUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Miscellaneous Utility class.
    /// </summary>
    public static class MiscUtils
    {
        /// <summary>
        /// Generates the random string.
        /// </summary>
        /// <param name="maxSize">Size of the max.</param>
        /// <param name="characterSet">The character set.</param>
        /// <returns>Generated random string.</returns>
        /// <exception cref="System.ArgumentNullException">characterSet</exception>
        public static string GenerateRandomString(int maxSize, string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890")
        {
            if (string.IsNullOrWhiteSpace(characterSet))
            {
                throw new ArgumentNullException("characterSet");
            }

            char[] chars = characterSet.ToCharArray();
            return GenerateRandomString(maxSize, chars);
        }

        /// <summary>
        /// Generates the random string.
        /// </summary>
        /// <param name="maxSize">Size of the max.</param>
        /// <param name="characterSet">The character set.</param>
        /// <returns>Generated random string.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">maxSize</exception>
        /// <exception cref="System.ArgumentNullException">characterSet</exception>
        public static string GenerateRandomString(int maxSize, char[] characterSet)
        {
            if (maxSize < 1)
            {
                throw new ArgumentOutOfRangeException("maxSize");
            }

            if (characterSet == null)
            {
                throw new ArgumentNullException("characterSet");
            }

            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }

            StringBuilder result = new StringBuilder(maxSize);
            for (int i = 0; i < data.Length; i++)
            {
                byte b = data[i];
                result.Append(characterSet[b % (characterSet.Length - 1)]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Longs to base20 string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Base20 string.</returns>
        public static string LongToBase20String(long value)
        {
            return LongToBaseString(value, new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' });
        }

        /// <summary>
        /// Bases the string to long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="baseChars">The base chars.</param>
        /// <returns>Base string.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// value
        /// or
        /// baseChars
        /// </exception>
        public static long BaseStringToLong(string value, char[] baseChars)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (baseChars == null) throw new ArgumentNullException("baseChars");

            int targetBase = baseChars.Length;
            long result = 0;

            for (int i = value.Length - 1, j = 0; i >= 0; i--, j++)
            {
                int digit = Array.IndexOf(baseChars, value[i]);
                result += digit * (long)Math.Pow(targetBase, j);
            }

            return result;
        }

        /// <summary>
        /// Longs to base string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="baseChars">The base chars.</param>
        /// <returns>Base string.</returns>
        /// <exception cref="System.ArgumentNullException">baseChars</exception>
        public static string LongToBaseString(long value, char[] baseChars)
        {
            if (baseChars == null) throw new ArgumentNullException("baseChars");

            // 64 is the worst case buffer size for base 2 and long.MaxValue
            const int bufferSize = 64;
            int i = bufferSize;
            char[] buffer = new char[i];
            int targetBase = baseChars.Length;

            do
            {
                buffer[--i] = baseChars[value % targetBase];
                value = value / targetBase;
            }
            while (value > 0);

            char[] result = new char[bufferSize - i];
            Array.Copy(buffer, i, result, 0, bufferSize - i);

            return new string(result);
        }

        /// <summary>
        /// Converts to integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Integer value.</returns>
        public static int? ToInt32(string @value)
        {
            if (@value.IsNullOrWhiteSpace())
            {
                return null;
            }

            int result;
            if (int.TryParse(@value, out result))
            {
                return result;
            }

            return null;
        }
    }
}