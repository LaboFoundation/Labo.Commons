// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlUtils.cs" company="Labo">
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
//   Defines the XmlUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;
    using System.Globalization;
    using System.Xml;

    using Labo.Common.Resources;
    using Labo.Common.Utils.Exceptions;

    /// <summary>
    /// 
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="culture"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetNodeAttributeValue<TValue>(XmlNode node, string propertyName, TValue defaultValue, CultureInfo culture = null)
        {
            culture = CultureUtils.GetCurrentCultureIfNull(culture);

            string value;
            if (TryGetNodeAttributeValue(node, propertyName, out value))
            {
                return ConvertUtils.ChangeType<TValue>(value, culture);
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <param name="culture"> </param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        /// <exception cref="XmlUtilsException"></exception>
        public static TValue GetNodeAttributeValue<TValue>(XmlNode node, string propertyName, CultureInfo culture = null)
        {
            if (node == null) throw new XmlUtilsException(Strings.XmlUtils_GetNodeProperty_Error);

            culture = CultureUtils.GetCurrentCultureIfNull(culture);

            return ConvertUtils.ChangeType<TValue>(GetNodeAttributeValue(node, propertyName, null), culture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string GetNodeAttributeValue(XmlNode node, string propertyName, string defaultValue)
        {
            string value;
            if (TryGetNodeAttributeValue(node, propertyName, out value))
            {
                return value;
            }
            return defaultValue;
        }

        private static bool TryGetNodeAttributeValue(XmlNode node, string propertyName, out string value)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            if (node.Attributes != null && (node.Attributes[propertyName] != null))
            {
                value = node.Attributes[propertyName].Value;
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="XmlUtilsException"></exception>
        public static XmlAttribute AppendAttribute(XmlNode element, string name, string value)
        {
            if (element == null) throw new ArgumentNullException("element");
            if (value == null) throw new ArgumentNullException("value");

            if (element.OwnerDocument == null)
            {
                throw new XmlUtilsException(Strings.XmlUtils_AppendAttribute_element_OwnerDocument_is_null);
            }
            if (element.Attributes == null)
            {
                throw new XmlUtilsException(Strings.XmlUtils_AppendAttribute_element_Attributes_is_null);
            }

            XmlAttribute attribute = element.OwnerDocument.CreateAttribute(name);
            attribute.Value = value;
            element.Attributes.Append(attribute);
            return attribute;
        }

        /// <summary>
        /// Creates a new XmlNode node with the name specified and appends to the parent.
        /// </summary>
        /// <param name="node">Parent node.</param>
        /// <param name="childNodeName">Name of the newly created xml node.</param>
        /// <returns>Returns the created new child node.</returns>
        public static XmlNode AppendToNode(XmlNode node, string childNodeName)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (childNodeName == null) throw new ArgumentNullException("childNodeName");

            if (node.OwnerDocument == null)
            {
                throw new XmlUtilsException(Strings.XmlUtils_AppendNodeToParent_parentNode_OwnerDocument_is_null);
            }
            XmlNode childNode = node.OwnerDocument.CreateNode(XmlNodeType.Element, childNodeName, string.Empty);
            node.AppendChild(childNode);
            return childNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="childNodeName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static XmlNode AppendToNode(XmlDocument xmlDocument, string childNodeName)
        {
            if (xmlDocument == null) throw new ArgumentNullException("xmlDocument");
            if (childNodeName == null) throw new ArgumentNullException("childNodeName");

            XmlNode childNode = xmlDocument.CreateNode(XmlNodeType.Element, childNodeName, string.Empty);
            xmlDocument.AppendChild(childNode);
            return childNode;
        }
    }
}
