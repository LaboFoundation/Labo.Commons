using System;
using System.Xml;

using Labo.Common.Exceptions;
using Labo.Common.Resources;
using Labo.Common.Utils.Exceptions;

namespace Labo.Common.Utils
{
    public static class XmlUtils
    {     
        public static T GetNodeProperty<T>(XmlNode node, string propertyName, T defaultValue)
        {
            return ConvertUtils.ChangeType<T>(GetNodePropertyValue(node, propertyName, defaultValue));
        }

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

        public static T GetNodeProperty<T>(XmlNode node, string propertyName)
        {
            if (node == null)
            {
                CoreLevelException nodeException = new CoreLevelException("21");
                throw new XmlUtilsException(Strings.XmlUtils_GetNodeProperty_Error, nodeException);
            }
            
            object value = GetNodePropertyValue(node, propertyName, null);
            if (value != null)
            {
                return ConvertUtils.ChangeType<T>(value);
            }

            CoreLevelException attException = new CoreLevelException("19");
            attException.Data.Add("20", propertyName);
            XmlUtilsException ex = new XmlUtilsException("19", attException);
            throw ex;
        }

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
