// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyUtils.cs" company="Labo">
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
//  <summary>
//   Defines the AssemblyUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Security.Permissions;
    using System.Text;

    using Labo.Common.Culture;
    using Labo.Common.Resources;
    using Labo.Common.Utils.Exceptions;

    /// <summary>
    /// Assembly Utils class.
    /// </summary>
    public static class AssemblyUtils
    {
        /// <summary>
        /// Gets the assembly time.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Assembly last write time.</returns>
        /// <exception cref="System.ArgumentNullException">assembly</exception>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        [FileIOPermission(SecurityAction.Demand, Unrestricted = true)]
        public static DateTime GetAssemblyTime(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            AssemblyName assemblyName = assembly.GetName();
            return File.GetLastWriteTime(new Uri(assemblyName.CodeBase).LocalPath);
        }

        /// <summary>
        /// Gets the embedded resource string.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>Embedded resource as string.</returns>
        public static string GetEmbeddedResourceString(string resourceName)
        {
            return GetEmbeddedResourceString(Assembly.GetExecutingAssembly(), resourceName, EncodingHelper.CurrentCultureEncoding);
        }

        /// <summary>
        /// Gets the embedded resource string.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>Embedded resource as string.</returns>
        public static string GetEmbeddedResourceString(string resourceName, Encoding encoding)
        {
            return GetEmbeddedResourceString(Assembly.GetExecutingAssembly(), resourceName, encoding);
        }

        /// <summary>
        /// Gets the embedded resource string.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>Embedded resource as string.</returns>
        public static string GetEmbeddedResourceString(Assembly assembly, string resourceName)
        {
            return GetEmbeddedResourceString(assembly, resourceName, EncodingHelper.CurrentCultureEncoding);
        }

        /// <summary>
        /// Gets the embedded resource string.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>Embedded resource as string.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// assembly
        /// or
        /// resourceName
        /// or
        /// encoding
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        public static string GetEmbeddedResourceString(Assembly assembly, string resourceName, Encoding encoding)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (resourceName == null) throw new ArgumentNullException("resourceName");
            if (encoding == null) throw new ArgumentNullException("encoding");

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
                else
                {
                    throw new FileNotFoundException(resourceName);
                }
            }
            catch (FileNotFoundException ex)
            {
                AssemblyUtilsException assemblyUtilsException = new AssemblyUtilsException(string.Format(CultureInfo.CurrentCulture, Strings.AssemblyUtils_GetEmbededResourceString_embedded_resource_not_found, resourceName), ex);
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
        /// Gets the embedded resource binary.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>Embedded resource as byte array.</returns>
        public static byte[] GetEmbeddedResourceBinary(string resourceName)
        {
            return GetEmbeddedResourceBinary(Assembly.GetExecutingAssembly(), resourceName);
        }

        /// <summary>
        /// Gets the embedded resource binary.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>Embedded resource as byte array.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// assembly
        /// or
        /// resourceName
        /// </exception>
        public static byte[] GetEmbeddedResourceBinary(Assembly assembly, string resourceName)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (resourceName == null) throw new ArgumentNullException("resourceName");

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
