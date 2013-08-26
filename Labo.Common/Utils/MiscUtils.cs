using System;
using System.Security.Cryptography;
using System.Text;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class MiscUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxSize"></param>
        /// <param name="characterSet"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GenerateRandomString(int maxSize, string characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890")
        {
            if (string.IsNullOrEmpty(characterSet))
            {
                throw new ArgumentNullException("characterSet");
            }

            char[] chars = characterSet.ToCharArray();
            return GenerateRandomString(maxSize, chars);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxSize"></param>
        /// <param name="characterSet"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
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
                result.Append(characterSet[b%(characterSet.Length - 1)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LongToBase20String(long value)
        {
            return LongToBaseString(value,
                                   new[]
                                       {
                                           '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F',
                                           'G', 'H', 'I', 'J'
                                       });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="baseChars"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="baseChars"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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
        /// To the int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int? ToInt32(string @value)
        {
            if (@value.IsNullOrWhiteSpace())
            {
                return null;
            }
            int result;
            if(int.TryParse(@value, out result))
            {
                return result;
            }
            return null;
        }
    }
}