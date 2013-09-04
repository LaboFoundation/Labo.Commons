// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinqUtils.cs" company="Labo">
//
// The MIT License (MIT)
//
// Copyright (c) 2013 Bora Akgun
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the LinqUtils.cs type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Linq Utils class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public static class LinqUtils
    {
        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <typeparam name="TProp">The type of the prop.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>Member name of the expression.</returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetMemberName<TType, TProp>(Expression<Func<TType, TProp>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            MemberInfo memberInfo = GetMemberInfo(expression);

            return memberInfo.Name;
        }

        /// <summary>
        /// Gets the member info.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <typeparam name="TMember">The type of the member.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>MemberInfo of the expression.</returns>
        /// <exception cref="System.ArgumentNullException">expression</exception>
        /// <exception cref="System.InvalidOperationException">Expression body must be MemberExpression, MethodCallExpression or UnaryExpression</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static MemberInfo GetMemberInfo<TType, TMember>(Expression<Func<TType, TMember>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            MemberExpression body = expression.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    body = unaryExpression.Operand as MemberExpression;
                }
            }

            if (body == null)
            {
                MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;
                if (methodCallExpression != null)
                {
                    return methodCallExpression.Method;
                }
            }

            if (body == null)
            {
                throw new InvalidOperationException("Expression body must be MemberExpression, MethodCallExpression or UnaryExpression");
            }

            return body.Member;
        }
    }
}
