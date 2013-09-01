using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Labo.Common.Utils;
using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class XmlUtilsFixture
    {
        [Test]
        public void GetNodeAttributeValue()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode productNode = CreateProductNode(xmlDocument);

            XmlAttribute idAttribute = xmlDocument.CreateAttribute("id");
            idAttribute.Value = "1";
            productNode.Attributes.Append(idAttribute);

            XmlAttribute priceAttribute = xmlDocument.CreateAttribute("price");
            priceAttribute.Value = "10.5";
            productNode.Attributes.Append(priceAttribute);

            XmlAttribute priceTRAttribute = xmlDocument.CreateAttribute("priceTR");
            priceTRAttribute.Value = "10,5";
            productNode.Attributes.Append(priceTRAttribute);

            Assert.AreEqual("1", XmlUtils.GetNodeAttributeValue(productNode, "id", null));
            Assert.AreEqual(null, XmlUtils.GetNodeAttributeValue(productNode, "ID", null));
            Assert.AreEqual("10.5", XmlUtils.GetNodeAttributeValue(productNode, "price", null));
            
            Assert.AreEqual(1, XmlUtils.GetNodeAttributeValue<int>(productNode, "id", CultureInfo.InvariantCulture));
            Assert.AreEqual(0, XmlUtils.GetNodeAttributeValue(productNode, "ID", 0, CultureInfo.InvariantCulture));
            Assert.AreEqual(10.5M, XmlUtils.GetNodeAttributeValue(productNode, "price", 0M, CultureInfo.InvariantCulture));
            Assert.AreEqual(10.5M, XmlUtils.GetNodeAttributeValue(productNode, "priceTR", 0M, new CultureInfo("tr-TR")));
        }

        [Test]
        public void AppendAttribute()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode productNode = CreateProductNode(xmlDocument);

            XmlUtils.AppendAttribute(productNode, "id", "1");

            Assert.IsNotNull(productNode.Attributes);
            Assert.AreEqual(1, productNode.Attributes.Count);
            Assert.AreEqual("1", productNode.Attributes["id"].Value);

            XmlUtils.AppendAttribute(productNode, "price", "10.5");

            Assert.IsNotNull(productNode.Attributes);
            Assert.AreEqual(2, productNode.Attributes.Count);
            Assert.AreEqual("1", productNode.Attributes["id"].Value);
            Assert.AreEqual("10.5", productNode.Attributes["price"].Value);

            XmlUtils.AppendAttribute(productNode, "price", "15.5");

            Assert.IsNotNull(productNode.Attributes);
            Assert.AreEqual(2, productNode.Attributes.Count);
            Assert.AreEqual("1", productNode.Attributes["id"].Value);
            Assert.AreEqual("15.5", productNode.Attributes["price"].Value);
        }

        [Test]
        public void AppendNode()
        {
            XmlNode productsNode = XmlUtils.AppendToNode(new XmlDocument(), "products");

            Assert.IsNotNull(productsNode.OwnerDocument);
            Assert.AreEqual(1, productsNode.OwnerDocument.ChildNodes.Count);
            Assert.AreEqual("products", productsNode.OwnerDocument.ChildNodes[0].Name);

            XmlNode productNode = XmlUtils.AppendToNode(productsNode, "product");

            Assert.AreEqual(1, productsNode.ChildNodes.Count);
            Assert.AreEqual("product", productsNode.ChildNodes[0].Name);

            XmlNode productVariants = XmlUtils.AppendToNode(productNode, "variants");

            Assert.AreEqual(1, productNode.ChildNodes.Count);
            Assert.AreEqual("variants", productNode.ChildNodes[0].Name);
            
            XmlNode productVariant1 = XmlUtils.AppendToNode(productVariants, "variant");
            XmlUtils.AppendAttribute(productVariant1, "Name", "Color");

            XmlNode productVariant2 = XmlUtils.AppendToNode(productVariants, "variant");
            XmlUtils.AppendAttribute(productVariant2, "Name", "Size");

            Assert.AreEqual(2, productVariants.ChildNodes.Count);
            Assert.AreEqual("variant", productVariants.ChildNodes[0].Name);
            Assert.IsNotNull(productVariants.ChildNodes[0].Attributes);
            Assert.AreEqual(1, productVariants.ChildNodes[0].Attributes.Count);
            Assert.AreEqual("Color", productVariants.ChildNodes[0].Attributes["Name"].Value);

            Assert.AreEqual("variant", productVariants.ChildNodes[1].Name);
            Assert.IsNotNull(productVariants.ChildNodes[1].Attributes);
            Assert.AreEqual(1, productVariants.ChildNodes[1].Attributes.Count);
            Assert.AreEqual("Size", productVariants.ChildNodes[1].Attributes["Name"].Value);
        }

        private static XmlNode CreateProductNode(XmlDocument xmlDocument)
        {
            XmlNode docNode = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(docNode);

            XmlNode productsNode = xmlDocument.CreateElement("products");
            xmlDocument.AppendChild(productsNode);

            XmlNode productNode = xmlDocument.CreateElement("product");
            productsNode.AppendChild(productNode);

            return productNode;
        }
    }
}
