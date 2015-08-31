namespace Labo.Common.Tests.Expression
{
    using System;
    using System.Collections.Generic;

    using Labo.Common.Expression;
    using Labo.Common.Reflection;
    using Labo.Common.Reflection.Exceptions;

    using NUnit.Framework;

    [TestFixture]
    public class ExpressionHelperTestFixture
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

        private class PropertyTestClass
        {
            private int m_ReadWriteProperty = 1;

            public int ReadOnlyProperty
            {
                get
                {
                    return 1;
                }
            }

            public int ReadWriteProperty
            {
                get
                {
                    return m_ReadWriteProperty;
                }
                set
                {
                    m_ReadWriteProperty = value;
                }
            }

            public int WriteOnlyProperty
            {
                set
                {
                    m_ReadWriteProperty = value;
                }
            }

            public string StringProperty { get; set; }
        }

        private class GetMethodTestClass
        {
            private void PrivateVoidMethodWithNoParameters()
            {
            }

            public void PublicVoidMethodWithNoParameters()
            {
            }

            private void PrivateVoidMethodWithParameters(int i)
            {
            }

            private void PrivateVoidMethodWithParameters(int i, long j)
            {
            }

            public void PublicVoidMethodWithParameters(int i)
            {
            }

            public void PublicVoidMethodWithParameters(int i, long j)
            {
            }
        }

        private class CallMethodTestClass
        {
            public void PublicVoidMethodWithNoParameters()
            {
            }

            private int PrivateMethodWithNoParameters()
            {
                return 10;
            }

            public int PublicMethodWithNoParameters()
            {
                return 100;
            }

            private int PrivateMethodWithParameters(int i)
            {
                return i * 10;
            }

            private long PrivateMethodWithParameters(int i, long j)
            {
                return i * j;
            }

            public int PublicMethodWithParameters(int i)
            {
                return i * 100;
            }

            public long PublicMethodWithParameters(int i, long j)
            {
                return i + j;
            }
        }

        [Test]
        public void SetPropertyValue()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            propertyTestClass.ReadWriteProperty = 1;

            ExpressionHelper.SetPropertyValue(propertyTestClass, "ReadWriteProperty", 2);

            Assert.AreEqual(2, propertyTestClass.ReadWriteProperty);
        }

        [Test]
        public void SetStringPropertyValueToNull()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            propertyTestClass.StringProperty = "Test";

            ReflectionHelper.SetPropertyValue(propertyTestClass, "StringProperty", null);

            Assert.AreEqual(null, propertyTestClass.StringProperty);
        }

        [Test]
        public void GetPropertyValue()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            propertyTestClass.ReadWriteProperty = 2;

            Assert.AreEqual(2, ExpressionHelper.GetPropertyValue(propertyTestClass, "ReadWriteProperty"));
        }

        [Test, ExpectedException(typeof(ReflectionHelperException))]
        public void GetPropertyValueThrowExceptionWhenNoGetMethod()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            ExpressionHelper.GetPropertyValue(propertyTestClass, "WriteOnlyProperty");
        }

        [Test, ExpectedException(typeof(ReflectionHelperException))]
        public void SetPropertyValueThrowExceptionWhenNoSetMethod()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            ExpressionHelper.SetPropertyValue(propertyTestClass, "ReadOnlyProperty", 1);
        }

        [Test, ExpectedException(typeof(ReflectionHelperException))]
        public void SetPropertyValueThrowsExceptionWhenNonImplicitlyConvertableValueIsSet()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            ExpressionHelper.SetPropertyValue(propertyTestClass, "WriteOnlyProperty", 1F);
        }

        [Test]
        public void CallMethod()
        {
            CallMethodTestClass testClass = new CallMethodTestClass();
            Assert.AreEqual(testClass.PublicMethodWithNoParameters(), ExpressionHelper.CallMethod(testClass, "PublicMethodWithNoParameters"));
            Assert.AreEqual(testClass.PublicMethodWithParameters(5), ExpressionHelper.CallMethod(testClass, "PublicMethodWithParameters", 5));
            Assert.AreEqual(testClass.PublicMethodWithParameters(5, 20L), ExpressionHelper.CallMethod(testClass, "PublicMethodWithParameters", 5, 20L));
            Assert.DoesNotThrow(() => ExpressionHelper.CallMethod(testClass, "PublicVoidMethodWithNoParameters"));
        }
    }
}
