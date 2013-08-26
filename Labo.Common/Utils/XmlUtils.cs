using System;
using System.Xml;

using Labo.Common.Resources;
using Labo.Common.Utils.Exceptions;

namespace Labo.Common.Utils
{
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
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetNodeProperty<T>(XmlNode node, string propertyName, T defaultValue)
        {
            return ConvertUtils.ChangeType<T>(GetNodePropertyValue(node, propertyName, defaultValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static object GetNodePropertyValue(XmlNode node, string propertyName, object defaultValue)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            if (node.Attributes != null && (node.Attributes[propertyName] != null))
            {
                return node.Attributes[propertyName].Value;
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="XmlUtilsException"></exception>
        public static T GetNodeProperty<T>(XmlNode node, string propertyName)
        {
            if (node == null)
            {
                throw new XmlUtilsException(Strings.XmlUtils_GetNodeProperty_Error);
            }
            
            return ConvertUtils.ChangeType<T>(GetNodePropertyValue(node, propertyName, null));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="XmlUtilsException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static XmlAttribute AppendAttribute<T>(XmlNode element, string name, object value, object defaultValue)
        {
            if (element == null) throw new ArgumentNullException("element");
            if (value == null || value == defaultValue)
            {
                return null;
            }

            if (element.OwnerDocument == null)
            {
                throw new XmlUtilsException(Strings.XmlUtils_AppendAttribute_element_OwnerDocument_is_null);
            }
            XmlAttribute attribute = element.OwnerDocument.CreateAttribute(name);
            attribute.Value = ConvertUtils.ChangeType<T>(value).ToString();
            if (element.Attributes != null)
            {
                element.Attributes.Append(attribute);
            }
            return attribute;
        }

        /// <summary>
        /// Creates a new XmlNode node with the name specified and appends to the parent.
        /// </summary>
        /// <param name="parentNode">Parent node.</param>
        /// <param name="childNodeName">Name of the newly created xml node.</param>
        /// <returns>Returns the created new child node.</returns>
        public static XmlNode AppendNodeToParent(XmlNode parentNode, string childNodeName)
        {
            if (parentNode == null) throw new ArgumentNullException("parentNode");
            if (parentNode.OwnerDocument == null)
            {
                throw new XmlUtilsException(Strings.XmlUtils_AppendNodeToParent_parentNode_OwnerDocument_is_null);
            }
            XmlNode childNode = parentNode.OwnerDocument.CreateNode(XmlNodeType.Element, childNodeName, string.Empty);
            parentNode.AppendChild(childNode);
            return childNode;
        }
    }
}
