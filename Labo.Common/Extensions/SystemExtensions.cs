using Labo.Common.Utils;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class SystemExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="string"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string @string)
        {
            return string.IsNullOrWhiteSpace(@string);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="string1"></param>
        /// <param name="string2"></param>
        /// <returns></returns>
        public static bool EqualsOrdinalIgnoreCase(this string string1, string string2)
        {
            return StringUtils.EqualsOrdinalIgnoreCase(string1, string2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string ToStringInvariant(this object @object)
        {
            return ObjectUtils.ToStringInvariant(@object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="string"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string @string, params object[] args)
        {
            return StringUtils.Format(@string, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="string"></param>
        /// <param name="formatProvider"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string @string, IFormatProvider formatProvider, params object[] args)
        {
            return string.Format(formatProvider, @string, args);
        }
    }
}
