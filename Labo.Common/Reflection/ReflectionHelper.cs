// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" company="Labo">
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
//   The reflection helper class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Labo.Common.Utils;

    /// <summary>
    /// The reflection helper class.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// The default property info binding flags.
        /// </summary>
        public const BindingFlags DEFAULT_PROPERTY_INFO_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// The default method info binding flags.
        /// </summary>
        public const BindingFlags DEFAULT_METHOD_INFO_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// The property access item class.
        /// </summary>
        private sealed class PropertyAccessItem
        {
            /// <summary>
            /// Gets or sets the getter delegate.
            /// </summary>
            /// <value>
            /// The getter delegate.
            /// </value>
            public MemberGetter Getter { get; set; }

            /// <summary>
            /// Gets or sets the setter delegate.
            /// </summary>
            /// <value>
            /// The setter delegate.
            /// </value>
            public MemberSetter Setter { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether property [can read].
            /// </summary>
            /// <value>
            ///   <c>true</c> if property [can read]; otherwise, <c>false</c>.
            /// </value>
            public bool CanRead { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether property [can write].
            /// </summary>
            /// <value>
            ///   <c>true</c> if property [can write]; otherwise, <c>false</c>.
            /// </value>
            public bool CanWrite { get; set; }
        }

        /// <summary>
        /// The dynamic method cache
        /// </summary>
        private static readonly DynamicMethodCache s_DynamicMethodCache;

        /// <summary>
        /// Initializes static members of the <see cref="ReflectionHelper"/> class.
        /// </summary>
        static ReflectionHelper()
        {
            s_DynamicMethodCache = new DynamicMethodCache();
        }

        /// <summary>
        /// Calls the method.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The method return value.</returns>
        public static object CallMethod(object @object, string methodName, params object[] parameters)
        {
            return CallMethod(@object, methodName, DEFAULT_METHOD_INFO_BINDING_FLAGS, parameters);
        }

        /// <summary>
        /// Calls the method.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The method return value.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// object
        /// or
        /// methodName
        /// </exception>
        public static object CallMethod(object @object, string methodName, BindingFlags bindingFlags, params object[] parameters)
        {
            if (@object == null)
            {
                throw new ArgumentNullException("object");
            }

            if (methodName.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("methodName");
            }

            Type objectType = @object.GetType();
            MethodInfo methodInfo = GetMethodInfo(objectType, methodName, bindingFlags, parameters);
            return CallMethod(@object, methodInfo, parameters);
        }

        /// <summary>
        /// Calls the method.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Method return value.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// object
        /// or
        /// methodInfo
        /// </exception>
        public static object CallMethod(object @object, MethodInfo methodInfo, params object[] parameters)
        {
            if (@object == null)
            {
                throw new ArgumentNullException("object");
            }

            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            if (parameters == null)
            {
                parameters = new object[] { null };
            }

            Type objectType = @object.GetType();

            return s_DynamicMethodCache.GetOrAddDelegate(methodInfo, () => DynamicMethodHelper.EmitMethodInvoker(objectType, methodInfo), DynamicMethodCacheStrategy.Temporary)(@object, parameters);
        }

        /// <summary>
        /// Calls the method.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Method return value.</returns>
        public static object CallMethod(object @object, string methodName, params NamedParameterWithValue[] parameters)
        {
            return CallMethod(@object, methodName, DEFAULT_METHOD_INFO_BINDING_FLAGS, parameters);
        }

        /// <summary>
        /// Calls the method.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The method return value.</returns>
        /// <exception cref="System.ArgumentNullException">object</exception>
        public static object CallMethod(object @object, string methodName, BindingFlags bindingFlags, params NamedParameterWithValue[] parameters)
        {
            if (@object == null)
            {
                throw new ArgumentNullException("object");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            int parametersLength = parameters.Length;
            IDictionary<string, Type> parameterNames = new Dictionary<string, Type>(parametersLength);
            object[] parameterValues = new object[parametersLength];
            for (int i = 0; i < parametersLength; i++)
            {
                NamedParameterWithValue namedParameterWithValue = parameters[i];
                parameterNames.Add(namedParameterWithValue.Name, namedParameterWithValue.Type);
                parameterValues[i] = namedParameterWithValue.Value;
            }

            return CallMethod(@object, GetMethodByName(@object.GetType(), bindingFlags, methodName, parameterNames), parameterValues);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <exception cref="System.ArgumentNullException">
        /// object
        /// or
        /// propertyName
        /// </exception>
        public static void SetPropertyValue(object @object, string propertyName, object value, BindingFlags bindingFlags = DEFAULT_PROPERTY_INFO_BINDING_FLAGS)
        {
            if (@object == null)
            {
                throw new ArgumentNullException("object");
            }

            if (propertyName.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("propertyName");
            }

            Type type = @object.GetType();
            PropertyInfo propertyInfo = GetPropertyInfo(propertyName, bindingFlags, type);

            SetPropertyValue(@object, propertyInfo, value);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="value">The value.</param>
        public static void SetPropertyValue(object @object, PropertyInfo propertyInfo, object value)
        {
            if (@object == null)
            {
                throw new ArgumentNullException("object");
            }

            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            Type type = @object.GetType();
            PropertyAccessItem propertyAccessItem = GetPropertyAccessItem(type, propertyInfo);
            propertyAccessItem.Setter(@object, value);
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <returns>The property value</returns>
        /// <exception cref="System.ArgumentNullException">
        /// object
        /// or
        /// propertyName
        /// </exception>
        public static object GetPropertyValue(object @object, string propertyName, BindingFlags bindingFlags = DEFAULT_PROPERTY_INFO_BINDING_FLAGS)
        {
            if (@object == null)
            {
                throw new ArgumentNullException("object");
            }

            if (propertyName.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("propertyName");
            }

            Type type = @object.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingFlags);
            if (propertyInfo == null)
            {
                // TODO: throw exception.
            }

            return GetPropertyValue(@object, propertyInfo);
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>The property value</returns>
        /// <exception cref="System.ArgumentNullException">
        /// object
        /// or
        /// propertyInfo
        /// </exception>
        public static object GetPropertyValue(object @object, PropertyInfo propertyInfo)
        {
            if (@object == null)
            {
                throw new ArgumentNullException("object");
            }

            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            Type type = @object.GetType();
            PropertyAccessItem propertyAccessItem = GetPropertyAccessItem(type, propertyInfo);
            return propertyAccessItem.Getter(@object);
        }

        /// <summary>
        /// Gets the method by name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The method info.</returns>
        public static MethodInfo GetMethodByName(Type type, BindingFlags bindingFlags, string methodName, IDictionary<string, Type> parameters)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (methodName == null)
            {
                throw new ArgumentNullException("methodName");
            }

            MethodInfo[] methodInfos = type.GetMethods(bindingFlags);
            for (int i = 0; i < methodInfos.Length; i++)
            {
                MethodInfo methodInfo = methodInfos[i];
                if (methodInfo.Name == methodName)
                {
                    ParameterInfo[] parameterInfos = methodInfo.GetParameters();

                    if (parameters == null)
                    {
                        if (parameterInfos.Length == 0)
                        {
                            return methodInfo;
                        }

                        continue;
                    }

                    if (parameterInfos.Length != parameters.Count)
                    {
                        continue;
                    }

                    bool parameterFound = true;
                    foreach (KeyValuePair<string, Type> parameter in parameters)
                    {
                        parameterFound = false;

                        for (int j = 0; j < parameterInfos.Length; j++)
                        {
                            ParameterInfo parameterInfo = parameterInfos[j];
                            if (parameterInfo.Name == parameter.Key && parameterInfo.ParameterType == parameter.Value)
                            {
                                parameterFound = true;
                                break;
                            }
                        }

                        if (!parameterFound)
                        {
                            break;
                        }
                    }

                    if (!parameterFound)
                    {
                        continue;
                    }

                    return methodInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <param name="type">The type.</param>
        /// <returns>The property info.</returns>
        private static PropertyInfo GetPropertyInfo(string propertyName, BindingFlags bindingFlags, Type type)
        {
            PropertyInfo propertyInfo = type.GetProperty(propertyName, bindingFlags);
            if (propertyInfo == null)
            {
                // TODO: throw exception.
            }

            return propertyInfo;
        }

        /// <summary>
        /// Gets the property access item.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>The property access item.</returns>
        private static PropertyAccessItem GetPropertyAccessItem(Type objectType, PropertyInfo propertyInfo)
        {
            return s_DynamicMethodCache.GetOrAddDelegate(propertyInfo, () => CreatePropertyAccessItem(objectType, propertyInfo), DynamicMethodCacheStrategy.Temporary);
        }

        /// <summary>
        /// Creates the property access item.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>The property access item.</returns>
        private static PropertyAccessItem CreatePropertyAccessItem(Type objectType, PropertyInfo propertyInfo)
        {
            PropertyAccessItem propertyAccessItem = new PropertyAccessItem
                                                        {
                                                            CanRead = propertyInfo.CanRead,
                                                            CanWrite = propertyInfo.CanWrite
                                                        };
            if (propertyInfo.CanRead)
            {
                propertyAccessItem.Getter = DynamicMethodHelper.EmitPropertyGetter(objectType, propertyInfo);
            }

            if (propertyInfo.CanWrite)
            {
                propertyAccessItem.Setter = DynamicMethodHelper.EmitPropertySetter(objectType, propertyInfo);
            }

            return propertyAccessItem;
        }

        /// <summary>
        /// Gets the method information.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The method info.</returns>
        internal static MethodInfo GetMethodInfo(Type objectType, string methodName, BindingFlags bindingFlags, params object[] parameters)
        {
            if (parameters == null)
            {
                parameters = new object[] { null };
            }

            Type[] parameterTypes = GetParameterTypes(parameters);
            return objectType.GetMethod(methodName, bindingFlags, null, parameterTypes, null);
        }

        /// <summary>
        /// Gets the parameter types.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The parameter types.</returns>
        private static Type[] GetParameterTypes(IList<object> parameters)
        {
            int parametersCount = parameters.Count;
            if (parametersCount == 0)
            {
                return Type.EmptyTypes;
            }

            Type[] parameterTypes = new Type[parametersCount];
            for (int i = 0; i < parametersCount; i++)
            {
                object parameter = parameters[i];
                parameterTypes[i] = TypeUtils.GetType(parameter);
            }

            return parameterTypes;
        }
    }
}
