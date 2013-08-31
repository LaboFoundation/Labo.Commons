using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="inherit"> </param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static TAttribute GetCustomAttribute<TAttribute, TType, TProp>(Expression<Func<TType, TProp>> expression, bool inherit = false) 
            where TAttribute : class
        {
            if (expression == null) throw new ArgumentNullException("expression");

            return GetCustomAttribute<TAttribute>(LinqUtils.GetMemberInfo(expression), inherit);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(MemberInfo memberInfo)
            where TAttribute : class
        {
            return GetCustomAttribute<TAttribute>(memberInfo, false);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="parameterInfo">The parameter info.</param>
        /// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(ParameterInfo parameterInfo)
            where TAttribute : class
        {
            return GetCustomAttribute<TAttribute>(parameterInfo, false);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="parameterInfo">The parameter info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static TAttribute GetCustomAttribute<TAttribute>(ParameterInfo parameterInfo, bool inherit)
            where TAttribute : class 
        {
            if (parameterInfo == null) throw new ArgumentNullException("parameterInfo");

            object[] attributes = parameterInfo.GetCustomAttributes(typeof (TAttribute), inherit);
            if(attributes.Length == 0)
            {
                return null;
            }
            return (TAttribute) attributes[0];
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="inherit"></param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool HasCustomAttribute<TAttribute, TType, TProp>(Expression<Func<TType, TProp>> expression, bool inherit = false)
           where TAttribute : class
        {
            if (expression == null) throw new ArgumentNullException("expression");

            return HasCustomAttribute<TAttribute>(LinqUtils.GetMemberInfo(expression), inherit);
        }

        /// <summary>
        /// Determines whether [has custom attribute] [the specified member info].
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
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
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="inherit"></param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IList<TAttribute> GetCustomAttributes<TAttribute, TType, TProp>(Expression<Func<TType, TProp>> expression, bool inherit = false)
           where TAttribute : class
        {
            if (expression == null) throw new ArgumentNullException("expression");

            return GetCustomAttributes<TAttribute>(LinqUtils.GetMemberInfo(expression), inherit);
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <param name="inherit"></param>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

            ConstructorInfo constructor =
                type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

            return constructor != null && !constructor.IsPrivate;
        }
    }
}