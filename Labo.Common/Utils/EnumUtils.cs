using System;
using System.Collections.Generic;
using System.Globalization;
using Labo.Common.Exceptions;
using Labo.Common.Resources;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Gets the names and values.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <typeparam name="TUnderlyingType">The type of the underlying type.</typeparam>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IDictionary<string, TUnderlyingType> GetNamesAndValues<TEnum, TUnderlyingType>()
            where TEnum : struct
        {
            return GetNamesAndValues<TUnderlyingType>(typeof(TEnum));
        }

        /// <summary>
        /// Gets the names and values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IDictionary<string, ulong> GetNamesAndValues<T>() where T : struct
        {
            return GetNamesAndValues<ulong>(typeof(T));
        }

        /// <summary>
        /// Gets the names and values.
        /// </summary>
        /// <typeparam name="TUnderlyingType">The type of the underlying type.</typeparam>
        /// <param name="enumType">Type of the enum.</param>
        /// <exception cref="ArgumentException">Underlying Type Must Be Enum</exception>
        /// <exception cref="ArgumentNullException">enumType cannot be null</exception>
        /// <returns></returns>
        public static IDictionary<string, TUnderlyingType> GetNamesAndValues<TUnderlyingType>(Type enumType)
        {
            if (enumType == null) throw new ArgumentNullException("enumType");
            if (!enumType.IsEnum) throw new ArgumentException(Strings.EnumUtils_GetNamesAndValues_Underlying_Type_Must_Be_Enum, "enumType");

            Type conversionType = typeof(TUnderlyingType);
            try
            {
                Array values = Enum.GetValues(enumType);
                string[] names = Enum.GetNames(enumType);
                IDictionary<string, TUnderlyingType> dictionary = new Dictionary<string, TUnderlyingType>();
                for (int i = 0; i < values.Length; i++)
                {
                    dictionary.Add(names[i], (TUnderlyingType)Convert.ChangeType(values.GetValue(i), conversionType, CultureInfo.InvariantCulture));
                }
                return dictionary;
            }
            catch (Exception exception)
            {
                CoreLevelException coreEx = new CoreLevelException(Strings.EnumUtils_GetNamesAndValues_An_Error_Occurred_While_Getting_Names_and_Values_of_Enum_Type, exception);
                coreEx.Data.Add("TYPE", enumType.ToString());
                coreEx.Data.Add("UNDERLYINGTYPE", conversionType.ToString());
                throw coreEx;
            }
        }

        /// <summary>
        /// Parses the specified enum member name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <returns></returns>
        public static T Parse<T>(string enumMemberName) where T : struct
        {
            return Parse<T>(enumMemberName, false);
        }

        /// <summary>
        /// Parses the specified enum member name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static T Parse<T>(string enumMemberName, bool ignoreCase) where T : struct
        {
            return (T)Parse(typeof(T), enumMemberName, ignoreCase);
        }

        /// <summary>
        /// Parses the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <exception cref="ArgumentNullException">type and enumMemberName cannot be null</exception>
        /// <exception cref="ArgumentException">type argument must be enum type</exception>
        /// <returns></returns>
        public static object Parse(Type type, string enumMemberName, bool ignoreCase)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (enumMemberName == null) throw new ArgumentNullException("enumMemberName");
            if (!type.IsEnum) throw new ArgumentException(Strings.EnumUtils_GetNamesAndValues_Underlying_Type_Must_Be_Enum, "type");

            try
            {
                return Enum.Parse(type, enumMemberName, ignoreCase);
            }
            catch (Exception exception)
            {
                CoreLevelException coreEx = new CoreLevelException(Strings.EnumUtils_Parse_An_Error_Occurred_While_Parsing_Enum_Value, exception);
                coreEx.Data.Add("VALUE", enumMemberName);
                coreEx.Data.Add("TYPE", type.ToString());
                coreEx.Data.Add("IGNORECASE", ignoreCase.ToString(CultureInfo.InvariantCulture));
                throw coreEx;
            }
        }

        /// <summary>
        /// Tries to parse the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryParse<T>(string enumMemberName, out T value) where T : struct
        {
            return TryParse(enumMemberName, false, out value);
        }

        /// <summary>
        /// Tries to parse the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryParse<T>(string enumMemberName, bool ignoreCase, out T value) where T : struct
        {
            object objectValue;
            bool parsed = TryParse(typeof(T), enumMemberName, ignoreCase, out objectValue);
            value = objectValue == null ? default(T) : (T)objectValue;
            return parsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="enumMemberName"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
        public static bool TryParse(Type enumType, string enumMemberName, bool ignoreCase, out object value)
        {
            if (enumType == null) throw new ArgumentNullException("enumType");
            if (enumMemberName == null) throw new ArgumentNullException("enumMemberName");
            if (!enumType.IsEnum) throw new ArgumentException(Strings.EnumUtils_GetNamesAndValues_Underlying_Type_Must_Be_Enum, "enumType");

            try
            {
                string[] names = Enum.GetNames(enumType);
                for (int i = 0; i < names.Length; i++)
                {
                    string str = names[i];
                    bool @equals = ignoreCase ? str.Equals(enumMemberName, StringComparison.OrdinalIgnoreCase) : str.Equals(enumMemberName);
                    if (@equals)
                    {
                        value = Parse(enumType, enumMemberName, ignoreCase);
                        return true;
                    }
                }
                value = default(object);
                return false;
            }
            catch (Exception exception)
            {
                CoreLevelException coreEx = new CoreLevelException(Strings.EnumUtils_Parse_An_Error_Occurred_While_Parsing_Enum_Value, exception);
                coreEx.Data.Add("VALUE", enumMemberName);
                coreEx.Data.Add("TYPE", enumType.ToString());
                coreEx.Data.Add("IGNORECASE", ignoreCase.ToString(CultureInfo.InvariantCulture));
                throw coreEx;
            }
        }
    }
}