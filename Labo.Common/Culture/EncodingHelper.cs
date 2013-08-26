using System.Text;

namespace Labo.Common.Culture
{
    /// <summary>
    /// 
    /// </summary>
    public static class EncodingHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static Encoding CurrentCultureEncoding
        {
            get
            {
                return Encoding.GetEncoding(System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.ANSICodePage);                
            }
        }
    }
}
