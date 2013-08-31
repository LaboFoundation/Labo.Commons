using System;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class CryptoUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncryptMd5(string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            using(MD5 md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding encoder = new UTF8Encoding();
                byte[] returnValue = md5.ComputeHash(encoder.GetBytes(value));
                return Encoding.UTF8.GetString(returnValue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptMd5AsHexString(string input)
        {
            if (input == null) throw new ArgumentNullException("input");

            // step 1, calculate MD5 hash from input
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = md5.ComputeHash(inputBytes);

                // step 2, convert byte array to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                }
                return sb.ToString();
            }
        }
    }
}

