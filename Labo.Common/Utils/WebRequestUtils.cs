using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using Labo.Common.Exceptions;

namespace Labo.Common.Utils
{
    public static class WebRequestUtils
    {
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

        public static string PostData(Uri uri, NameValueCollection data)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;

                byte[] responsebytes = client.UploadValues(uri, "POST", data);
                return Encoding.UTF8.GetString(responsebytes);
            }
        }

        public static string PostData(string url, NameValueCollection data)
        {
            return PostData(new Uri(url), data);
        }

        public static string PostString(Uri uri, string data)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                return client.UploadString(uri, "POST", data);
            }
        }

        public static string PostString(string url, string data)
        {
            return PostString(new Uri(url), data);
        }

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
