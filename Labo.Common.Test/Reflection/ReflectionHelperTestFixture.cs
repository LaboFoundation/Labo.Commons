namespace Labo.Common.Tests.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Labo.Common.Reflection;
    using Labo.Common.Utils;

    using NUnit.Framework;

    [TestFixture]
    public class ReflectionHelperTestFixture
    {
        private static class StaticClass
        {
            public static int Random()
            {
                return new Random().Next();
            }
        }

        private class Parent
        {
            private readonly IList<Child> m_Children;

            public IList<Child> Children
            {
                get
                {
                    return m_Children;
                }
            }

            public Parent()
            {
                m_Children = new List<Child>();
            }

            public void AddChild(Child child)
            {
                Children.Add(child);
            }

            public void AddChild(Child child1, Child child2)
            {
                Children.Add(child1);
                Children.Add(child2);
            }

            public int GetChildCount()
            {
                return Children.Count;
            }
        }

        private class Child
        {
             
        }

        [Test]
        public void GetMethodInfoPublic()
        {
            MethodInfo methodInfo = ReflectionHelper.GetMethodInfo(typeof(Parent), "AddChild", BindingFlags.Public | BindingFlags.Instance, new Child());
            Assert.IsNotNull(methodInfo);
            Assert.AreEqual("AddChild", methodInfo.Name);
            Assert.AreEqual(1, methodInfo.GetParameters().Length);
            Assert.AreEqual(typeof(void), methodInfo.ReturnParameter.ParameterType);

            methodInfo = ReflectionHelper.GetMethodInfo(typeof(Parent), "AddChild", BindingFlags.Public | BindingFlags.Instance, new Child(), new Child());
            Assert.IsNotNull(methodInfo);
            Assert.AreEqual("AddChild", methodInfo.Name);
            Assert.AreEqual(2, methodInfo.GetParameters().Length);
            Assert.AreEqual(typeof(void), methodInfo.ReturnParameter.ParameterType);

            methodInfo = ReflectionHelper.GetMethodInfo(typeof(Parent), "GetChildCount", BindingFlags.Public | BindingFlags.Instance);
            Assert.IsNotNull(methodInfo);
            Assert.AreEqual("GetChildCount", methodInfo.Name);
            Assert.AreEqual(0, methodInfo.GetParameters().Length);
            Assert.AreEqual(typeof(int), methodInfo.ReturnParameter.ParameterType);
        }

        [Test]
        public void GetMethodInfoPublicStatic()
        {
            MethodInfo methodInfo = ReflectionHelper.GetMethodInfo(typeof(StaticClass), "Random", BindingFlags.Public | BindingFlags.Static);
            Assert.IsNotNull(methodInfo);
            Assert.AreEqual("Random", methodInfo.Name);
            Assert.AreEqual(0, methodInfo.GetParameters().Length);
            Assert.AreEqual(typeof(int), methodInfo.ReturnParameter.ParameterType);
        }
    }
}
