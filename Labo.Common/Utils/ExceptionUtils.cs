using System;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionUtils
    {
        /// <summary>
        /// Finds in the specified exception.
        /// </summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public static TException Find<TException>(Exception exception)
            where TException : Exception
        {
            while (exception != null && !(exception is TException))
            {
                exception = exception.InnerException;
            }

            return (TException)exception;
        }
    }
}
