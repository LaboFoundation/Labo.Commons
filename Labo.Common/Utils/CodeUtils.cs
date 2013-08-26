using System;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class CodeUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static bool TryCatch(Action action, Action<Exception> exceptionHandler = null)
        {
            if (action == null) throw new ArgumentNullException("action");

            try
            {
                action();
                return true;
            }
            catch(Exception ex)
            {
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
                return false;
            }
        }
    }
}
