// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebRequestUtils.cs" company="Labo">
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
//   Defines the WebRequestUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;

    using Labo.Common.Exceptions;

    /// <summary>
    /// Web request utility class.
    /// </summary>
    public static class WebRequestUtils
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="method">The method.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static string GetData(Uri uri, string method = "GET", NameValueCollection headers = null)
        {
            WebRequest request = WebRequest.Create(uri);
            request.Method = method;
            if (headers != null)
            {
                request.Headers.Add(headers);
            }

            using (WebResponse response = request.GetResponse())
            {
                string body;
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    body = sr.ReadToEnd();
                }

                return body;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="method">The method.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static string GetData(string url, string method = "GET", NameValueCollection headers = null)
        {
            return GetData(new Uri(url), method, headers);
        }

        /// <summary>
        /// Posts the data.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string PostData(Uri uri, NameValueCollection data)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;

                byte[] responsebytes = client.UploadValues(uri, "POST", data);
                return Encoding.UTF8.GetString(responsebytes);
            }
        }

        /// <summary>
        /// Posts the data.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string PostData(string url, NameValueCollection data)
        {
            return PostData(new Uri(url), data);
        }

        /// <summary>
        /// Posts the string.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string PostString(Uri uri, string data)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                return client.UploadString(uri, "POST", data);
            }
        }

        /// <summary>
        /// Posts the string.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string PostString(string url, string data)
        {
            return PostString(new Uri(url), data);
        }

        /// <summary>
        /// Downloads the data.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static byte[] DownloadData(Uri uri)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    return client.DownloadData(uri);
                }
            }
            catch (Exception ex)
            {
                CoreLevelException downloadDataException = new CoreLevelException("Error occured while downloading data", ex);
                downloadDataException.Data.Add("URL", uri);
                throw downloadDataException;
            }
        }

        /// <summary>
        /// Downloads the data.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static byte[] DownloadData(string url)
        {
            return DownloadData(new Uri(url));
        }
    }
}
