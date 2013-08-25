using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;

namespace Labo.Common.Web.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class UriUtils
    {
        /// <summary>
        /// Toes the absolute path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string ToAbsolutePath(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (path.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
            {
                path = VirtualPathUtility.ToAbsolute(path);
            }

            return path;
        }

        /// <summary>
        /// Toes the physical path.
        /// </summary>
        /// <param name="absolutePath">The absolute path.</param>
        /// <returns></returns>
        public static string ToPhysicalPath(string absolutePath)
        {
            return HttpContext.Current.Server.MapPath(absolutePath);
        }

        /// <summary>
        /// Gets the domain address from URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string GetDomainAddress(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            string hostAddress = uri.GetComponents(UriComponents.Host, UriFormat.SafeUnescaped);
            if (hostAddress.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
            {
                return hostAddress.Substring(4, hostAddress.Length - 4);
            }
            return hostAddress;
        }

        /// <summary>
        /// Gets the domain address.
        /// </summary>
        /// <returns></returns>
        public static string GetDomainAddress()
        {
            return GetDomainAddress(HttpContext.Current.Request.Url);
        }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static string GetBaseUrl(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            return uri.GetLeftPart(UriPartial.Authority);
        }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
        public static string GetBaseUrl()
        {
            return GetBaseUrl(HttpContext.Current.Request.Url);
        }

        /// <summary>
        /// Combines the specified base URL.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="urlPart">The URL part.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#")]
        public static string Combine(string baseUrl, string urlPart)
        {
            return Combine(new Uri(baseUrl), urlPart);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
        public static string Combine(Uri baseUri, string urlPart)
        {
            return new Uri(baseUri, urlPart).ToStringInvariant();
        }

        public static Uri Combine(Uri baseUri, Uri uriPart)
        {
            return new Uri(baseUri, uriPart);
        }

        /// <summary>
        /// Adds to query string.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public static string AddToQueryString(string url, string key, string value)
        {
            if (url == null) throw new ArgumentNullException("url");

            int iqs = url.IndexOf('?');
            return String.Format(CultureInfo.CurrentCulture, iqs == -1 ? "{0}?{1}={2}" : "{0}&{1}={2}", url, HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value));
        }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static NameValueCollection GetQueryString(string url)
        {
            if(url.IsNullOrWhiteSpace())
            {
                return new NameValueCollection(0);
            }
            Uri tempUri = new Uri(url);
            return GetQueryString(tempUri);
        }

        public static NameValueCollection GetQueryString(Uri tempUri)
        {
            if (tempUri == null) throw new ArgumentNullException("tempUri");

            string sQuery = tempUri.Query;
            return HttpUtility.ParseQueryString(sQuery);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
        public static string FullUrl(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = "~";
            }

            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);

            return FullUrl(path, uri);
        }

        public static string FullUrl(string path, Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");

            //If the URI is not already absolute, rebuild it based on the current request.
            if (!uri.IsAbsoluteUri)
            {
                Uri requestUrl = HttpContext.Current.Request.Url;
                UriBuilder builder = new UriBuilder(requestUrl.Scheme, requestUrl.Host, requestUrl.Port);

                builder.Path = VirtualPathUtility.ToAbsolute(path);
                uri = builder.Uri;
            }

            return uri.ToString();
        }
    }
}
