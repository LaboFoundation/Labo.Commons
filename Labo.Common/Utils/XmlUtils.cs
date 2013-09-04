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
    /// Xml utility class.
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// Gets the node attribute value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="node">The node.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The node attribute value.</returns>
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
        /// Gets the node attribute value.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="node">The node.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The node attribute value.</returns>
        public static TValue GetNodeAttributeValue<TValue>(XmlNode node, string propertyName, CultureInfo culture = null)
        {
            if (node == null) throw new XmlUtilsException(Strings.XmlUtils_GetNodeProperty_Error);

            culture = CultureUtils.GetCurrentCultureIfNull(culture);

            return ConvertUtils.ChangeType<TValue>(GetNodeAttributeValue(node, propertyName, null), culture);
        }

        /// <summary>
        /// Gets the node attribute value.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The node attribute value.</returns>
        public static string GetNodeAttributeValue(XmlNode node, string propertyName, string defaultValue)
        {
            string value;
            if (TryGetNodeAttributeValue(node, propertyName, out value))
            {
                return value;
            }

            return defaultValue;
        }

        /// <summary>
        /// Appends the attribute.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>appended attribute.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// element
        /// or
        /// value
        /// </exception>
        /// <exception cref="XmlUtilsException">element.OwnerDocument and element.Attributes connot be null.</exception>
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
        /// Appends to node.
        /// </summary>
        /// <param name="xmlDocument">The XML document.</param>
        /// <param name="childNodeName">Name of the child node.</param>
        /// <returns>Appended node.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// xmlDocument
        /// or
        /// childNodeName
        /// </exception>
        public static XmlNode AppendToNode(XmlDocument xmlDocument, string childNodeName)
        {
            if (xmlDocument == null) throw new ArgumentNullException("xmlDocument");
            if (childNodeName == null) throw new ArgumentNullException("childNodeName");

            XmlNode childNode = xmlDocument.CreateNode(XmlNodeType.Element, childNodeName, string.Empty);
            xmlDocument.AppendChild(childNode);
            return childNode;
        }

        /// <summary>
        /// Tries the get node attribute value.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if finds attribute value else <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// node
        /// or
        /// propertyName
        /// </exception>
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
    }
}
