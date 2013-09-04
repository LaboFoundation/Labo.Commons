// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationUtils.cs" company="Labo">
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
//   Defines the SerializationUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public static class SerializationUtils
    {
        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <param name="value">Object that is to be serialized to XML</param>
        /// <param name="culture"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>XML string</returns>
        public static string XmlSerializeObject(object value, CultureInfo culture = null)
        {
            if (value == null) throw new ArgumentNullException("value");

            culture = CultureUtils.GetCurrentCultureIfNull(culture);

            Type objectType = value.GetType();
            XmlSerializer xmlSerializer = new XmlSerializer(objectType);
            using (StringWriter stringWriter = new StringWriter(culture))
            {
                XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
                xmlSerializer.Serialize(xmlWriter, value);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Method to reconstruct an Object from XML string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="objectType"> </param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static object DeserializeXmlObject(string value, Type objectType)
        {
            if (value == null) throw new ArgumentNullException("value");
            if (objectType == null) throw new ArgumentNullException("objectType");

            XmlSerializer xmlSerializer = new XmlSerializer(objectType);
            using (TextReader textReader = new StringReader(value))
            {
                return xmlSerializer.Deserialize(textReader);
            }
        }

        /// <summary>
        /// Method to reconstruct an Object from XML string
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static TObject DeserializeXmlObject<TObject>(string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return (TObject) DeserializeXmlObject(value, typeof (TObject));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] BinarySerializeObject(object value)
        {
            if (value == null) throw new ArgumentNullException("value");

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                bf.Serialize(memoryStream, value);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="TObject"></typeparam>
        /// <returns></returns>
        public static TObject BinaryDeserializeObject<TObject>(byte[] data)
        {
            return (TObject)BinaryDeserializeObject(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static object BinaryDeserializeObject(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(data, 0, data.Length);
                memoryStream.Position = 0;

                return bf.Deserialize(memoryStream);
            }
        }
    }
}
