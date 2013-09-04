// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionUtils.cs" company="Labo">
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
//   Reflection Utility class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Reflection Utility class.
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the custom attribute.</typeparam>
        /// <typeparam name="TType">The type of the class.</typeparam>
        /// <typeparam name="TProperty">The type of the class property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>Custom attribute.</returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static TAttribute GetCustomAttribute<TAttribute, TType, TProperty>(Expression<Func<TType, TProperty>> expression, bool inherit = false) 
            where TAttribute : class
        {
            if (expression == null) throw new ArgumentNullException("expression");

            return GetCustomAttribute<TAttribute>(LinqUtils.GetMemberInfo(expression), inherit);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">Custom attribute type.</typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <returns>Custom attribute.</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(MemberInfo memberInfo)
            where TAttribute : class
        {
            return GetCustomAttribute<TAttribute>(memberInfo, false);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">Custom attribute type.</typeparam>
        /// <param name="parameterInfo">The parameter info.</param>
        /// <returns>Custom attribute.</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(ParameterInfo parameterInfo)
            where TAttribute : class
        {
            return GetCustomAttribute<TAttribute>(parameterInfo, false);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">Custom attribute type.</typeparam>
        /// <param name="parameterInfo">The parameter info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>Custom attribute.</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(ParameterInfo parameterInfo, bool inherit)
            where TAttribute : class 
        {
            if (parameterInfo == null) throw new ArgumentNullException("parameterInfo");

            object[] attributes = parameterInfo.GetCustomAttributes(typeof(TAttribute), inherit);
            if (attributes.Length == 0)
            {
                return null;
            }

            return (TAttribute)attributes[0];
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute">Custom attribute type.</typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>Custom attribute.</returns>
        public static TAttribute GetCustomAttribute<TAttribute>(MemberInfo memberInfo, bool inherit)
            where TAttribute : class
        {
            if (memberInfo == null) throw new ArgumentNullException("memberInfo");

            object[] attributes = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit);
            if (attributes.Length == 0)
            {
                return null;
            }

            return (TAttribute)attributes[0];
        }

        /// <summary>
        /// Determines whether [has custom attribute] [the specified expression].
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <typeparam name="TProperty">The type of the prop.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>
        ///   <c>true</c> if [has custom attribute] [the specified expression]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool HasCustomAttribute<TAttribute, TType, TProperty>(Expression<Func<TType, TProperty>> expression, bool inherit = false)
           where TAttribute : class
        {
            if (expression == null) throw new ArgumentNullException("expression");

            return HasCustomAttribute<TAttribute>(LinqUtils.GetMemberInfo(expression), inherit);
        }

        /// <summary>
        /// Determines whether [has custom attribute] [the specified member info].
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>
        ///   <c>true</c> if [has custom attribute] [the specified member info]; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool HasCustomAttribute<TAttribute>(MemberInfo memberInfo, bool inherit)
           where TAttribute : class
        {
            return GetCustomAttribute<TAttribute>(memberInfo, inherit) != null;
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TType">The type of the class.</typeparam>
        /// <typeparam name="TProperty">The type of the class property.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>Custom attributes.</returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        public static IList<TAttribute> GetCustomAttributes<TAttribute, TType, TProperty>(Expression<Func<TType, TProperty>> expression, bool inherit = false)
           where TAttribute : class
        {
            if (expression == null) throw new ArgumentNullException("expression");

            return GetCustomAttributes<TAttribute>(LinqUtils.GetMemberInfo(expression), inherit);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>Custom attributes.</returns>
        /// <exception cref="System.ArgumentNullException">memberInfo</exception>
        public static IList<TAttribute> GetCustomAttributes<TAttribute>(MemberInfo memberInfo, bool inherit = false)
            where TAttribute : class
        {
            if (memberInfo == null) throw new ArgumentNullException("memberInfo");

            object[] attributes = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit);
            List<TAttribute> result = new List<TAttribute>(attributes.Length);
            for (int i = 0; i < attributes.Length; i++)
            {
                result.Add((TAttribute)attributes[i]);
            }

            return result;
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="parameterInfo">The parameter info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>Custom attributes.</returns>
        /// <exception cref="System.ArgumentNullException">parameterInfo</exception>
        public static IList<TAttribute> GetCustomAttributes<TAttribute>(ParameterInfo parameterInfo, bool inherit = false)
            where TAttribute : class
        {
            if (parameterInfo == null) throw new ArgumentNullException("parameterInfo");

            object[] attributes = parameterInfo.GetCustomAttributes(typeof(TAttribute), inherit);
            List<TAttribute> result = new List<TAttribute>(attributes.Length);
            for (int i = 0; i < attributes.Length; i++)
            {
                result.Add((TAttribute)attributes[i]);
            }

            return result;
        }

        /// <summary>
        /// Determines whether [has visible default constructor] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [has visible default constructor] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasVisibleDefaultConstructor(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

            return constructor != null && !constructor.IsPrivate;
        }
    }
}