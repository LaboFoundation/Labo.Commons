using System;

namespace Labo.Common.Utils
{
    public static class CodeUtils
    {
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
