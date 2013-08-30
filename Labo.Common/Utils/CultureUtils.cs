using System.Globalization;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class CultureUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static CultureInfo GetCurrentCultureIfNull(CultureInfo culture)
        {
            return ObjectUtils.IsNull(culture, () => CultureInfo.CurrentCulture);
        }
    }
}