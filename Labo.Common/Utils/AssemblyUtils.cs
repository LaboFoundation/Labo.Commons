using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;

using Labo.Common.Culture;
using Labo.Common.Resources;
using Labo.Common.Utils.Exceptions;

namespace Labo.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
        public static DateTime GetAssemblyTime(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            AssemblyName assemblyName = assembly.GetName();
            return File.GetLastWriteTime(new Uri(assemblyName.CodeBase).LocalPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string GetEmbededResourceString(string resourceName)
        {
            return GetEmbededResourceString(Assembly.GetExecutingAssembly(), resourceName, EncodingHelper.CurrentCultureEncoding);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetEmbededResourceString(string resourceName, Encoding encoding)
        {
            return GetEmbededResourceString(Assembly.GetExecutingAssembly(), resourceName, encoding);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static string GetEmbededResourceString(Assembly assembly, string resourceName)
        {
            return GetEmbededResourceString(assembly, resourceName, EncodingHelper.CurrentCultureEncoding);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="AssemblyUtilsException"></exception>
        public static string GetEmbededResourceString(Assembly assembly, string resourceName, Encoding encoding)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            string text = null;
            StreamReader streamReader = null;
            try
            {
                Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceName);
                if (manifestResourceStream != null)
                {
                    streamReader = new StreamReader(manifestResourceStream, encoding);
                    text = streamReader.ReadToEnd();
                }
            }
            catch(FileNotFoundException ex)
            {
                AssemblyUtilsException assemblyUtilsException = new AssemblyUtilsException(String.Format(CultureInfo.CurrentCulture, Strings.AssemblyUtils_GetEmbededResourceString_embedded_resource_not_found, resourceName), ex);
                assemblyUtilsException.Data.Add("ASSEMBLY", assembly.FullName);
                throw assemblyUtilsException;
            }
            finally
            {
                if (streamReader != null)
                {
                    ((IDisposable)streamReader).Dispose();
                    streamReader = null;
                }
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static byte[] GetEmbededResourceBinary(string resourceName)
        {
            return GetEmbededResourceBinary(Assembly.GetExecutingAssembly(), resourceName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] GetEmbededResourceBinary(Assembly assembly, string resourceName)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            byte[] resource = null;

            using (Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (manifestResourceStream != null)
                {
                    resource = new byte[manifestResourceStream.Length];
                    manifestResourceStream.Read(resource, 0, resource.Length);
                }
            }

            return resource;
        }
    }
}
