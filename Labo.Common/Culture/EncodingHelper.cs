using System.Text;

namespace Labo.Common.Culture
{
    public static class EncodingHelper
    {
        public static Encoding CurrentCultureEncoding
        {
            get
            {
                return Encoding.GetEncoding(System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.ANSICodePage);                
            }
        }
    }
}
