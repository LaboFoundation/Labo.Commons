using System;
using System.Globalization;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="object"></param>
        /// <param name="defaultValueCreator"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static TObject IsNull<TObject>(TObject @object, Func<TObject> defaultValueCreator)
            where TObject: class
        {
            if (defaultValueCreator == null) throw new ArgumentNullException("defaultValueCreator");

            if (@object == null)
            {
                return defaultValueCreator();
            }
            return @object;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="object"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static TObject Cast<TObject>(object @object)
        {
            return (TObject) @object;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string ToStringInvariant(object @object)
        {
            return Convert.ToString(@object, CultureInfo.InvariantCulture);
        }
    }
}
