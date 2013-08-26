using System;
using System.Linq.Expressions;

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
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static string GetProperyName<T, TProp>(Expression<Func<T, TProp>> expression)
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
                throw new InvalidOperationException("Expression body must be MemberExpression or UnaryExpression");
            }

            return body.Member.Name;
        }
    }
}
