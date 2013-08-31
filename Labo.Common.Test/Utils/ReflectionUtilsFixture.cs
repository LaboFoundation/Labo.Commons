using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Labo.Common.Utils;

using NUnit.Framework;

namespace Labo.Common.Tests.Utils
{
    [TestFixture]
    public class ReflectionUtilsFixture
    {
        private class TestClass
        {
            [Display(Name = "Prop1")]
            public string Prop1 { get; set; }

            [Display(Name = "Method1")]
            public string Method1([Display(Name = "arg")]string arg)
            {
                return arg;
            }
        }

        private class PrivateConstructorClass
        {
            private PrivateConstructorClass()
            {
                
            } 
        }

        [Test]
        public void GetCustomAttribute()
        {
            DisplayAttribute displayAttribute = ReflectionUtils.GetCustomAttribute<DisplayAttribute, TestClass, string>(x => x.Prop1);
            Assert.IsNotNull(displayAttribute);
            Assert.AreEqual("Prop1", displayAttribute.Name);

            displayAttribute = ReflectionUtils.GetCustomAttribute<DisplayAttribute, TestClass, string>(x => x.Method1(null));
            Assert.IsNotNull(displayAttribute);
            Assert.AreEqual("Method1", displayAttribute.Name);
        }

        [Test]
        public void GetCustomAttributeByMemberInfo()
        {
            DisplayAttribute displayAttribute = ReflectionUtils.GetCustomAttribute<DisplayAttribute>(typeof(TestClass).GetMember("Prop1")[0]);
            Assert.IsNotNull(displayAttribute);
            Assert.AreEqual("Prop1", displayAttribute.Name);

            displayAttribute = ReflectionUtils.GetCustomAttribute<DisplayAttribute>(typeof(TestClass).GetMember("Method1")[0]);
            Assert.IsNotNull(displayAttribute);
            Assert.AreEqual("Method1", displayAttribute.Name);
        }

        [Test]
        public void GetCustomAttributeByParameterInfo()
        {
            DisplayAttribute displayAttribute = ReflectionUtils.GetCustomAttribute<DisplayAttribute>(typeof(TestClass).GetMethod("Method1").GetParameters()[0]);
            Assert.IsNotNull(displayAttribute);
            Assert.AreEqual("arg", displayAttribute.Name);
        }

        [Test]
        public void GetCustomAttributes()
        {
            IList<DisplayAttribute> displayAttributes = ReflectionUtils.GetCustomAttributes<DisplayAttribute, TestClass, string>(x => x.Prop1);
            Assert.IsNotNull(displayAttributes);
            Assert.AreEqual(1, displayAttributes.Count);
            Assert.AreEqual("Prop1", displayAttributes[0].Name);

            displayAttributes = ReflectionUtils.GetCustomAttributes<DisplayAttribute, TestClass, string>(x => x.Method1(null));
            Assert.IsNotNull(displayAttributes);
            Assert.AreEqual(1, displayAttributes.Count);
            Assert.AreEqual("Method1", displayAttributes[0].Name);
        }

        [Test]
        public void GetCustomAttributesByMemberInfo()
        {
            IList<DisplayAttribute> displayAttributes = ReflectionUtils.GetCustomAttributes<DisplayAttribute>(typeof(TestClass).GetMember("Prop1")[0]);
            Assert.IsNotNull(displayAttributes);
            Assert.AreEqual(1, displayAttributes.Count);
            Assert.AreEqual("Prop1", displayAttributes[0].Name);

            displayAttributes = ReflectionUtils.GetCustomAttributes<DisplayAttribute>(typeof(TestClass).GetMember("Method1")[0]);
            Assert.IsNotNull(displayAttributes);
            Assert.AreEqual(1, displayAttributes.Count);
            Assert.AreEqual("Method1", displayAttributes[0].Name);
        }

        [Test]
        public void GetCustomAttributesByParameterInfo()
        {
            IList<DisplayAttribute> displayAttributes = ReflectionUtils.GetCustomAttributes<DisplayAttribute>(typeof(TestClass).GetMethod("Method1").GetParameters()[0]);
            Assert.IsNotNull(displayAttributes);
            Assert.AreEqual(1, displayAttributes.Count);
            Assert.AreEqual("arg", displayAttributes[0].Name);
        }

        [Test]
        public void HasCustomAttribute()
        {
            Assert.AreEqual(true, ReflectionUtils.HasCustomAttribute<DisplayAttribute, TestClass, string>(x => x.Prop1));
            Assert.AreEqual(true, ReflectionUtils.HasCustomAttribute<DisplayAttribute, TestClass, string>(x => x.Method1(null)));
        }

        [Test]
        public void HasVisibleDefaultConstructor()
        {
            Assert.AreEqual(true, ReflectionUtils.HasVisibleDefaultConstructor(typeof(TestClass)));
            Assert.AreEqual(false, ReflectionUtils.HasVisibleDefaultConstructor(typeof(PrivateConstructorClass)));
        }
    }
}