// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpRequestUtils.cs" company="Labo">
//   The MIT License (MIT)
//   
//   Copyright (c) 2013 Bora Akgun
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to
//   use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//   the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Http request utility class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Web.Utils
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    /// <summary>
    /// Http request utility class.
    /// </summary>
    public static class HttpRequestUtils
    {
        /// <summary>
        /// The AJAX REQUEST HEADER KEY
        /// </summary>
        private const string AJAX_REQUEST_HEADER_KEY = "X-Requested-With";

        /// <summary>
        /// The AJAX REQUEST HEADER VALUE
        /// </summary>
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
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
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
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsJsonRequest(HttpRequestBase request)
        {
            if (request == null) throw new ArgumentNullException("request");

            return IsJsonRequest(request.ContentType);
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

        /// <summary>
        /// Determines whether [is json request] [the specified content type].
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        ///   <c>true</c> if [is json request] [the specified content type]; otherwise, <c>false</c>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private static bool IsJsonRequest(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                return false;
            }

            return contentType.IndexOf(ContentType.JSON, StringComparison.OrdinalIgnoreCase) != -1;
        }
    }
}
