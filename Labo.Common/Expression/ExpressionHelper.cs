// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionHelper.cs" company="Labo">
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
//   The expression helper class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Expression
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Labo.Common.Reflection;
    using Labo.Common.Reflection.Exceptions;
    using Labo.Common.Resources;

    /// <summary>
    /// The expression helper class.
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// The dynamic method cache
        /// </summary>
        private static readonly DynamicMethodCache s_DynamicMethodCache = new DynamicMethodCache();

        /// <summary>
        /// Calls the method.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The method return value.</returns>
        public static object CallMethod(object @object, string methodName, params object[] parameters)
        {
            return CallMethod(@object, methodName, ReflectionHelper.DEFAULT_METHOD_INFO_BINDING_FLAGS, parameters);
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
            MethodInfo methodInfo = ReflectionHelper.GetMethodInfo(objectType, methodName, bindingFlags, parameters);
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

            // TODO: cache method parameters.
            ParameterInfo[] methodParameters = methodInfo.GetParameters();
            Type[] parameterTypes = new Type[methodParameters.Length];

            for (int i = 0; i < methodParameters.Length; i++)
            {
                ParameterInfo methodParameter = methodParameters[i];
                parameterTypes[i] = methodParameter.ParameterType;
                ReflectionHelper.CheckAreAssignable(methodInfo, parameters[i], methodParameter.ParameterType);
            }

            if (methodParameters.Length != parameters.Length)
            {
                throw new ReflectionHelperException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionHelper_CallMethod_Incorrect_number_of_arguments, methodInfo));
            }

            Type objectType = @object.GetType();

            return s_DynamicMethodCache.GetOrAddDelegate(new DynamicMethodInfo(objectType, MemberTypes.Method, methodInfo.Name, parameterTypes), () => CompileMethodFunc(objectType, methodInfo, methodParameters), DynamicMethodCacheStrategy.Temporary)(@object, parameters);
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
        public static void SetPropertyValue(object @object, string propertyName, object value, BindingFlags bindingFlags = ReflectionHelper.DEFAULT_PROPERTY_INFO_BINDING_FLAGS)
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
            PropertyInfo propertyInfo = ReflectionHelper.GetPropertyInfo(propertyName, bindingFlags, type);

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

            if (!propertyInfo.CanWrite)
            {
                throw new ReflectionHelperException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionHelper_SetPropertyValue_the_property_has_no_set_method, propertyInfo.Name, type.AssemblyQualifiedName));
            }

            ReflectionHelper.PropertyAccessItem propertyAccessItem = GetPropertyAccessItem(type, propertyInfo);

            //if (!TypeUtils.IsImplicitlyConvertible(valueType, propertyInfo.PropertyType))
            //{
            //    throw new ReflectionHelperException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionHelper_SetPropertyValue_type_is_not_implicitly_convertable, valueType, propertyInfo.PropertyType));
            //}

            ReflectionHelper.CheckAreAssignable(propertyInfo, value, propertyInfo.PropertyType);

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
        public static object GetPropertyValue(object @object, string propertyName, BindingFlags bindingFlags = ReflectionHelper.DEFAULT_PROPERTY_INFO_BINDING_FLAGS)
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
            PropertyInfo propertyInfo = ReflectionHelper.GetPropertyInfo(propertyName, bindingFlags, type);

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

            if (!propertyInfo.CanRead)
            {
                throw new ReflectionHelperException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionHelper_GetPropertyValue_property_has_no_get_method, propertyInfo.Name, type.AssemblyQualifiedName));
            }

            ReflectionHelper.PropertyAccessItem propertyAccessItem = GetPropertyAccessItem(type, propertyInfo);
            return propertyAccessItem.Getter(@object);
        }

        /// <summary>
        /// Gets the property access item.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>The property access item.</returns>
        private static ReflectionHelper.PropertyAccessItem GetPropertyAccessItem(Type objectType, PropertyInfo propertyInfo)
        {
            return s_DynamicMethodCache.GetOrAddDelegate(new DynamicMethodInfo(objectType, MemberTypes.Method, propertyInfo.Name, new[] { propertyInfo.PropertyType }), () => CreatePropertyAccessItem(objectType, propertyInfo), DynamicMethodCacheStrategy.Temporary);
        }

        /// <summary>
        /// Creates the property access item.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>The property access item.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed. Suppression is OK here.")]
        internal static ReflectionHelper.PropertyAccessItem CreatePropertyAccessItem(Type objectType, PropertyInfo propertyInfo)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException("objectType");
            }

            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            bool canRead = propertyInfo.CanRead;
            bool canWrite = propertyInfo.CanWrite;

            ReflectionHelper.PropertyAccessItem propertyAccessItem = new ReflectionHelper.PropertyAccessItem
            {
                CanRead = canRead,
                CanWrite = canWrite
            };

            if (canRead)
            {
                MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
                ParameterExpression thisParameterExpression = Expression.Parameter(typeof(object), "this");
                MethodCallExpression methodCallExpression = Expression.Call(Expression.Convert(thisParameterExpression, objectType), getMethodInfo);
                Expression callExpr = Expression.Convert(methodCallExpression, typeof(object));

                propertyAccessItem.Getter = Expression.Lambda<MemberGetter>(callExpr, thisParameterExpression).Compile();
            }

            if (canWrite)
            {
                MethodInfo setMethodInfo = propertyInfo.GetSetMethod(true);
                ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "value");
                ParameterExpression thisParameterExpression = Expression.Parameter(typeof(object), "this");
                MethodCallExpression methodCallExpression =
                    Expression.Call(
                        Expression.Convert(thisParameterExpression, objectType),
                        setMethodInfo,
                        new Expression[]
                            {
                                Expression.Convert(parameterExpression, setMethodInfo.GetParameters()[0].ParameterType)
                            });

                propertyAccessItem.Setter = Expression.Lambda<MemberSetter>(methodCallExpression, thisParameterExpression, parameterExpression).Compile();
            }

            return propertyAccessItem;
        }

        private static Func<object, object[], object> CompileMethodFunc(Type objectType, MethodInfo methodInfo, ParameterInfo[] methodParameters)
        {
            IList<ParameterExpression> parameterExpressions = new List<ParameterExpression>(methodParameters.Length);

            for (int i = 0; i < methodParameters.Length; i++)
            {
                parameterExpressions.Add(Expression.Parameter(methodParameters[i].ParameterType));
            }

            Type returnType = methodInfo.ReturnType;
            bool hasReturnType = returnType != typeof(void);

            ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), "parameters");
            ParameterExpression thisParameterExpression = Expression.Parameter(typeof(object), "this");
            Expression methodCallExpression = Expression.Call(Expression.Convert(thisParameterExpression, objectType), methodInfo, parameterExpressions.Cast<Expression>().ToArray());
            Expression returnExpression = hasReturnType ? Expression.Convert(methodCallExpression, typeof(object)) : (Expression)Expression.Block(methodCallExpression, Expression.Constant(null, typeof(object)));
            Expression callExpr = ReplaceConstantsWithArrayLookup(returnExpression, parameterExpressions.ToArray(), parameterExpression);
            
            return Expression.Lambda<Func<object, object[], object>>(callExpr, thisParameterExpression, parameterExpression).Compile();
        }

        private static Expression ReplaceConstantsWithArrayLookup(Expression expression, ParameterExpression[] parameters, ParameterExpression arrayParameter)
        {
            ParameterArrayIndexerVisitor indexer = new ParameterArrayIndexerVisitor(parameters, arrayParameter);

            return indexer.Visit(expression);
        }

        private sealed class ParameterArrayIndexerVisitor : ExpressionVisitor
        {
            private readonly List<ParameterExpression> m_ConstantExpressions;
            private readonly ParameterExpression m_ConstantsParameter;

            public ParameterArrayIndexerVisitor(ParameterExpression[] parameterExpressions, ParameterExpression arrayParameter)
            {
                m_ConstantExpressions = parameterExpressions.ToList();
                m_ConstantsParameter = arrayParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                int index = m_ConstantExpressions.IndexOf(node);

                if (index >= 0)
                {
                    return Expression.Convert(
                        Expression.ArrayIndex(
                            m_ConstantsParameter,
                            Expression.Constant(index, typeof(int))),
                        node.Type);
                }

                return base.VisitParameter(node);
            }
        }
    }
}
