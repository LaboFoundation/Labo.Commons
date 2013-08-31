using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class LinqUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetMemberName<TType, TProp>(Expression<Func<TType, TProp>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            MemberInfo memberInfo = GetMemberInfo(expression);

            return memberInfo.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <typeparam name="TType"></typeparam>
        /// <typeparam name="TMember"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
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
