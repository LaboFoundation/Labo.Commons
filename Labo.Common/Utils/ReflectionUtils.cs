using System;
using System.Collections.Generic;
using System.Reflection;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(MemberInfo memberInfo)
            where T : class
        {
            return GetCustomAttribute<T>(memberInfo, false);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterInfo">The parameter info.</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(ParameterInfo parameterInfo)
            where T : class
        {
            return GetCustomAttribute<T>(parameterInfo, false);
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterInfo">The parameter info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(ParameterInfo parameterInfo, bool inherit)
            where T : class 
        {
            if (parameterInfo == null) throw new ArgumentNullException("parameterInfo");

            object[] attributes = parameterInfo.GetCustomAttributes(typeof (T), inherit);
            if(attributes.Length == 0)
            {
                return null;
            }
            return (T) attributes[0];
        }

        /// <summary>
        /// Gets the custom attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(MemberInfo memberInfo, bool inherit)
            where T : class
        {
            if (memberInfo == null) throw new ArgumentNullException("memberInfo");

            object[] attributes = memberInfo.GetCustomAttributes(typeof(T), inherit);
            if (attributes.Length == 0)
            {
                return null;
            }
            return (T)attributes[0];
        }

        /// <summary>
        /// Determines whether [has custom attribute] [the specified member info].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns>
        ///   <c>true</c> if [has custom attribute] [the specified member info]; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static bool HasCustomAttribute<T>(MemberInfo memberInfo, bool inherit)
           where T : class
        {
            return GetCustomAttribute<T>(memberInfo, inherit) != null;
        }

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfo">The member info.</param>
        /// <param name="inherit">if set to <c>true</c> [inherit].</param>
        /// <returns></returns>
        public static IList<T> GetCustomAttributes<T>(MemberInfo memberInfo, bool inherit = false)
            where T : class
        {
            if (memberInfo == null) throw new ArgumentNullException("memberInfo");

            object[] attributes = memberInfo.GetCustomAttributes(typeof(T), inherit);
            List<T> result = new List<T>(attributes.Length);
            for (int i = 0; i < attributes.Length; i++)
            {
                result.Add((T)attributes[i]);
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
