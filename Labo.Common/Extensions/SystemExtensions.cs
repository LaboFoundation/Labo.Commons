﻿using Labo.Common.Utils;

namespace System
{
    public static class SystemExtensions
    {
        public static bool IsNullOrEmpty(this string @string)
        {
            return string.IsNullOrEmpty(@string);
        }

        public static bool IsNullOrWhiteSpace(this string @string)
        {
            return string.IsNullOrWhiteSpace(@string);
        }

        public static bool EqualsOrdinalIgnoreCase(this string string1, string string2)
        {
            return StringUtils.EqualsOrdinalIgnoreCase(string1, string2);
        }

        public static string ToStringInvariant(this object @object)
        {
            return ObjectUtils.ToStringInvariant(@object);
        }

        public static string FormatWith(this string @string, params object[] args)
        {
            return StringUtils.Format(@string, args);
        }

        public static string FormatWith(this string @string, IFormatProvider formatProvider, params object[] args)
        {
            return string.Format(formatProvider, @string, args);
        }
    }
}
