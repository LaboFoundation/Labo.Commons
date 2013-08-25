using System;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Labo.Common.Utils
{
    public static class SerializationUtils
    {
        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <param name="value">Object that is to be serialized to XML</param>
        /// <returns>XML string</returns>
        public static String SerializeObject(object value)
        {
            if (value == null) throw new ArgumentNullException("value");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                string xmlizedString = null;
                XmlSerializer xs = new XmlSerializer(value.GetType());
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xs.Serialize(xmlTextWriter, value);
                xmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                return xmlizedString;
            }
        }

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Method to reconstruct an Object from XML string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            value = value.Replace("utf-16", "utf-8");
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(value)))
            {
                return (T)xs.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        } 
    }
}
