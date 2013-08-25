using System;
using System.Web;

namespace Labo.Common.Web.Utils
{
    public static class HttpRequestUtils
    {
        private const string AJAX_REQUEST_HEADER_KEY = "X-Requested-With";
        private const string AJAX_REQUEST_HEADER_VALUE = "XMLHttpRequest";

        /// <summary>
        /// Determines whether [is ajax request] [the specified request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if [is ajax request] [the specified request]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAjaxRequest(HttpRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            string header = request.Headers[AJAX_REQUEST_HEADER_KEY];
            return !string.IsNullOrEmpty(header) && header.Contains(AJAX_REQUEST_HEADER_VALUE);
        }

        /// <summary>
        /// Determines whether [is ajax request] [the specified request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if [is ajax request] [the specified request]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAjaxRequest(HttpRequestBase request)
        {
            if (request == null) throw new ArgumentNullException("request");
            string header = request.Headers[AJAX_REQUEST_HEADER_KEY];
            return !string.IsNullOrEmpty(header) && header.Contains(AJAX_REQUEST_HEADER_VALUE);
        }

        /// <summary>
        /// Determines whether [is json request] [the specified request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if [is json request] [the specified request]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsJsonRequest(HttpRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            return IsJsonRequest(request.ContentType);
        }

        /// <summary>
        /// Determines whether [is json request] [the specified request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if [is json request] [the specified request]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsJsonRequest(HttpRequestBase request)
        {
            if (request == null) throw new ArgumentNullException("request");
            return IsJsonRequest(request.ContentType);
        }

        private static bool IsJsonRequest(string contentType)
        {
            if(string.IsNullOrEmpty(contentType))
            {
                return false;
            }
            return contentType.IndexOf(ContentType.JSON, StringComparison.OrdinalIgnoreCase) != -1;
        }

        /// <summary>
        /// Determines whether the specified request is post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if the specified request is post; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPost(HttpRequestBase request)
        {
            if (request == null) throw new ArgumentNullException("request");
            return request.HttpMethod == "POST";
        }
    }
}
