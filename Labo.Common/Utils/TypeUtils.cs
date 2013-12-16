// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeUtils.cs" company="Labo">
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
//   Defines the TypeUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Type utility class.
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// Determines whether [is number type] (long, int, short or byte).
        /// </summary>
        /// <param name="object">The @object.</param>
        /// <returns>
        ///   <c>true</c> if [is number type] [the specified @object]; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsNumberType(object @object)
        {
            return IsNumberType(GetType(@object));
        }

        /// <summary>
        /// Determines whether [is numeric type] (long, int, short, byte, float, double or decimal).
        /// </summary>
        /// <param name="object">The @object.</param>
        /// <returns>
        ///   <c>true</c> if [is numeric type] [the specified @object]; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsNumericType(object @object)
        {
            return IsNumericType(GetType(@object));
        }

        /// <summary>
        /// Determines whether [is number type] (long, int, short or byte).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="includeNullables">Include nullable types.</param>
        /// <returns>
        ///   <c>true</c> if [is number type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsNumberType(Type type, bool includeNullables = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (includeNullables && IsNullable(type))
            {
                type = type.GetGenericArguments()[0];
            }

            return type == typeof(long) || type == typeof(int) || type == typeof(short) || type == typeof(byte);
        }

        /// <summary>
        /// Determines whether [is numeric type] (long, int, short, byte, float, double or decimal).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="includeNullables">Include nullable types.</param>
        /// <returns>
        ///   <c>true</c> if [is numeric type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsNumericType(Type type, bool includeNullables = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (includeNullables && IsNullable(type))
            {
                type = type.GetGenericArguments()[0];
            }

            return type == typeof(long) || type == typeof(int) || type == typeof(short) || type == typeof(byte) ||
                   type == typeof(float) || type == typeof(double) || type == typeof(decimal);
        }

        /// <summary>
        /// Determines whether [is floating point number type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="includeNullables">Include nullable types.</param>
        /// <returns>
        ///   <c>true</c> if [is floating point number type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsFloatingPointNumberType(Type type, bool includeNullables = false)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (includeNullables && IsNullable(type))
            {
                type = type.GetGenericArguments()[0];
            }

            return type == typeof(float) || type == typeof(double) || type == typeof(decimal);
        }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <param name="object">The @object.</param>
        /// <returns>type.</returns>
        public static Type GetType(object @object)
        {
            if (@object == null)
            {
                return typeof(object);
            }

            return @object.GetType();
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <typeparam name="TType">The type.</typeparam>
        /// <param name="object">The object.</param>
        /// <returns>type.</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "object")]
        public static Type GetType<TType>(TType @object)
        {
            return typeof(TType);
        }

        /// <summary>
        /// Gets the default value of the object.
        /// </summary>
        /// <param name="object">The @object.</param>
        /// <returns>default value.</returns>
        public static object GetDefaultValue(object @object)
        {
            Type type = GetType(@object);
            return GetDefaultValueOfType(type);
        }

        /// <summary>
        /// Gets the default value of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>default value.</returns>
        public static object GetDefaultValueOfType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is nullable; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsNullable(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Makes the type nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Nullable type.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static Type MakeNullableType(Type type)
        {
            if (type.IsValueType && !IsNullable(type))
            {
                return typeof(Nullable<>).MakeGenericType(new[] { type });
            }

            return type;
        }

        /// <summary>
        /// Make the type non nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Generic argument if the type is nullable otherwise returns the type itself.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static Type MakeNonNullableType(Type type)
        {
            if (IsNullable(type))
            {
                return type.GetGenericArguments()[0];
            }

            return type;
        }

        /// <summary>
        /// Determines whether [is reference type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is reference type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public static bool IsReferenceType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return type.IsClass || type.IsInterface || type.IsAbstract;
        }

        /// <summary>
        /// Determines whether two types are assignable.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns><c>true</c> if the two types are assignable; otherwise, <c>false</c>.</returns>
        public static bool AreAssignable(Type source, Type destination)
        {
            return AreEquivalent(destination, source) || (!destination.IsValueType && !source.IsValueType && destination.IsAssignableFrom(source));
        }

        /// <summary>
        /// Determines whether two types are equivalent.
        /// </summary>
        /// <param name="type1">The type1.</param>
        /// <param name="type2">The type2.</param>
        /// <returns><c>true</c> if the two types are equivalent; otherwise, <c>false</c>.</returns>
        public static bool AreEquivalent(Type type1, Type type2)
        {
            return type1 == type2 || type1.IsEquivalentTo(type2);
        }

        /// <summary>
        /// Determines whether [is implicitly convertible] [the specified source].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>
        ///   <c>true</c> if [is implicitly convertible] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsImplicitlyConvertible(Type source, Type destination)
        {
            return AreEquivalent(source, destination) || IsImplicitNumericConvertible(source, destination) || IsImplicitReferenceConvertible(source, destination) || IsImplicitBoxingConvertible(source, destination) || IsImplicitNullableConvertible(source, destination);
        }

        /// <summary>
        /// Determines whether [is implicit boxing convertible] [the specified source].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>
        ///   <c>true</c> if [is implicit boxing convertible] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// destination
        /// </exception>
        public static bool IsImplicitBoxingConvertible(Type source, Type destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            return (source.IsValueType && (destination == typeof(object) || destination == typeof(ValueType))) || (source.IsEnum && destination == typeof(Enum));
        }

        /// <summary>
        /// Determines whether [is implicit reference convertible] [the specified source].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>
        ///   <c>true</c> if [is implicit reference convertible] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// destination
        /// </exception>
        public static bool IsImplicitReferenceConvertible(Type source, Type destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            return destination.IsAssignableFrom(source);
        }

        /// <summary>
        /// Determines whether [is implicit numeric convertible] [the specified source].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>
        ///   <c>true</c> if [is implicit numeric convertible] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsImplicitNumericConvertible(Type source, Type destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            TypeCode typeCodeSource = Type.GetTypeCode(source);
            TypeCode typeCodeDestination = Type.GetTypeCode(destination);
            switch (typeCodeSource)
            {
                case TypeCode.Char:
                    switch (typeCodeDestination)
                    {
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                        default:
                            return false;
                    }

                case TypeCode.SByte:
                    switch (typeCodeDestination)
                    {
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }

                    return false;
                case TypeCode.Byte:
                    switch (typeCodeDestination)
                    {
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                        default:
                            return false;
                    }

                case TypeCode.Int16:
                    switch (typeCodeDestination)
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }

                    return false;
                case TypeCode.UInt16:
                    switch (typeCodeDestination)
                    {
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                        default:
                            return false;
                    }

                case TypeCode.Int32:
                    switch (typeCodeDestination)
                    {
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }

                    return false;
                case TypeCode.UInt32:
                    switch (typeCodeDestination)
                    {
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }

                    return false;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    switch (typeCodeDestination)
                    {
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                        default:
                            return false;
                    }

                case TypeCode.Single:
                    return typeCodeDestination == TypeCode.Double;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether [is implicit nullable convertible] [the specified source].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>
        ///  <c>true</c> if [is implicit nullable convertible] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private static bool IsImplicitNullableConvertible(Type source, Type destination)
        {
            return IsNullable(destination) && IsImplicitlyConvertible(MakeNonNullableType(source), MakeNonNullableType(source));
        }
    }
}
