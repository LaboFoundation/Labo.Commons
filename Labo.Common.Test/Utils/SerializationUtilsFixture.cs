using System;
using System.Globalization;
using Labo.Common.Utils;
using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [Serializable]
    public class SerializationUtilsFixture
    {
        [Serializable]
        public class TestItem
        {
            public string Prop1 { get; set; } 
        }

        [Test]
        public void XmlSerializeObject()
        {
            string xmlSerializeObject = SerializationUtils.XmlSerializeObject(new TestItem
                {
                    Prop1 = "Prop1"
                }, CultureInfo.InvariantCulture);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-16\"?><TestItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Prop1>Prop1</Prop1></TestItem>", xmlSerializeObject);
        }

        [Test]
        public void DeserializeXmlObject()
        {
            TestItem testItem = SerializationUtils.DeserializeXmlObject<TestItem>("<?xml version=\"1.0\" encoding=\"utf-16\"?><TestItem xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><Prop1>Prop1</Prop1></TestItem>");
            
            Assert.IsNotNull(testItem);
            Assert.AreEqual("Prop1", testItem.Prop1);
        }

        [Test]
        public void BinaryDeserializeObject()
        {
            TestItem testItem = new TestItem {Prop1 = "Prop1"};
            TestItem binaryDeserializeObject = SerializationUtils.BinaryDeserializeObject<TestItem>(SerializationUtils.BinarySerializeObject(testItem));
            Assert.AreEqual(testItem.Prop1, binaryDeserializeObject.Prop1);
        }
    }
}
