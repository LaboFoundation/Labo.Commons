// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumUtils.cs" company="Labo">
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
//   Defines the EnumUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Labo.Common.Exceptions;
    using Labo.Common.Resources;

    /// <summary>
    /// Enumeration utility class.
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Gets the names and values.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <typeparam name="TUnderlyingType">The type of the underlying type.</typeparam>
        /// <returns>Names and values dictionary.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IDictionary<string, TUnderlyingType> GetNamesAndValues<TEnum, TUnderlyingType>()
            where TEnum : struct
        {
            return GetNamesAndValues<TUnderlyingType>(typeof(TEnum));
        }

        /// <summary>
        /// Gets the names and values.
        /// </summary>
        /// <typeparam name="TEnumType">Enum type.</typeparam>
        /// <returns>Names and values dictionary.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IDictionary<string, ulong> GetNamesAndValues<TEnumType>() where TEnumType : struct
        {
            return GetNamesAndValues<ulong>(typeof(TEnumType));
        }

        /// <summary>
        /// Gets the names and values.
        /// </summary>
        /// <typeparam name="TUnderlyingType">The type of the underlying type.</typeparam>
        /// <param name="enumType">Type of the enum.</param>
        /// <exception cref="ArgumentException">Underlying Type Must Be Enum</exception>
        /// <exception cref="ArgumentNullException">enumType cannot be null</exception>
        /// <returns>Names and values dictionary.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
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
        /// <typeparam name="TValue">Enum type.</typeparam>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <returns>Enum value.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static TValue Parse<TValue>(string enumMemberName) where TValue : struct
        {
            return Parse<TValue>(enumMemberName, false);
        }

        /// <summary>
        /// Parses the specified enum member name.
        /// </summary>
        /// <typeparam name="TValue">Enum type.</typeparam>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>Enum value.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static TValue Parse<TValue>(string enumMemberName, bool ignoreCase) where TValue : struct
        {
            return (TValue)Parse(typeof(TValue), enumMemberName, ignoreCase);
        }

        /// <summary>
        /// Parses the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <exception cref="ArgumentNullException">type and enumMemberName cannot be null</exception>
        /// <exception cref="ArgumentException">type argument must be enum type</exception>
        /// <returns>Enum object.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
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
        /// <typeparam name="TValue">Enum value.</typeparam>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if parses to enum value, otherwise <c>false</c></returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool TryParse<TValue>(string enumMemberName, out TValue value) where TValue : struct
        {
            return TryParse(enumMemberName, false, out value);
        }

        /// <summary>
        /// Tries to parse the specified type.
        /// </summary>
        /// <typeparam name="TValue">Enum type.</typeparam>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if parses to enum value, otherwise <c>false</c></returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool TryParse<TValue>(string enumMemberName, bool ignoreCase, out TValue value) where TValue : struct
        {
            object objectValue;
            bool parsed = TryParse(typeof(TValue), enumMemberName, ignoreCase, out objectValue);
            value = objectValue == null ? default(TValue) : (TValue)objectValue;
            return parsed;
        }

        /// <summary>
        ///  Tries to parse the specified type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="enumMemberName">Name of the enum member.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if parses to enum value, otherwise <c>false</c></returns>
        /// <exception cref="System.ArgumentNullException">
        /// enumType
        /// or
        /// enumMemberName
        /// </exception>
        /// <exception cref="System.ArgumentException">enumType</exception>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("Microsoft.Design", "CA1007:UseGenericsWhereAppropriate")]
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