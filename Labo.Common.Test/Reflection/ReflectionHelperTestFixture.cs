namespace Labo.Common.Tests.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq.Expressions;
    using System.Reflection;

    using Labo.Common.Reflection;
    using Labo.Common.Reflection.Exceptions;
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

        [Test]
        public void GetMethodInfoByName()
        {
            Assert.AreEqual(typeof(GetMethodTestClass).GetMethod("PrivateVoidMethodWithNoParameters", BindingFlags.Instance | BindingFlags.NonPublic), 
                            ReflectionHelper.GetMethodByName(typeof(GetMethodTestClass), BindingFlags.Instance | BindingFlags.NonPublic, "PrivateVoidMethodWithNoParameters"));

            Assert.AreEqual(typeof(GetMethodTestClass).GetMethod("PublicVoidMethodWithNoParameters", BindingFlags.Instance | BindingFlags.Public),
                            ReflectionHelper.GetMethodByName(typeof(GetMethodTestClass), BindingFlags.Instance | BindingFlags.Public, "PublicVoidMethodWithNoParameters"));

            Assert.AreEqual(typeof(GetMethodTestClass).GetMethod("PrivateVoidMethodWithParameters", BindingFlags.Instance | BindingFlags.NonPublic, null, new [] { typeof(int) }, null),
                            ReflectionHelper.GetMethodByName(typeof(GetMethodTestClass), BindingFlags.Instance | BindingFlags.NonPublic, "PrivateVoidMethodWithParameters", new Dictionary<string, Type>{ { "i", typeof(int) }  }));

            Assert.AreEqual(typeof(GetMethodTestClass).GetMethod("PublicVoidMethodWithParameters", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int) }, null),
                            ReflectionHelper.GetMethodByName(typeof(GetMethodTestClass), BindingFlags.Instance | BindingFlags.Public, "PublicVoidMethodWithParameters", new Dictionary<string, Type> { { "i", typeof(int) } }));

            Assert.AreEqual(typeof(GetMethodTestClass).GetMethod("PrivateVoidMethodWithParameters", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int), typeof(long) }, null),
                           ReflectionHelper.GetMethodByName(typeof(GetMethodTestClass), BindingFlags.Instance | BindingFlags.NonPublic, "PrivateVoidMethodWithParameters", new Dictionary<string, Type> { { "j", typeof(long) }, { "i", typeof(int) } }));

            Assert.AreEqual(typeof(GetMethodTestClass).GetMethod("PublicVoidMethodWithParameters", BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(int), typeof(long) }, null),
                            ReflectionHelper.GetMethodByName(typeof(GetMethodTestClass), BindingFlags.Instance | BindingFlags.Public, "PublicVoidMethodWithParameters", new Dictionary<string, Type> { { "j", typeof(long) }, { "i", typeof(int) } }));

            Assert.AreEqual(null,
                           ReflectionHelper.GetMethodByName(typeof(GetMethodTestClass), BindingFlags.Instance | BindingFlags.Public, "PublicVoidMethodWithParameters", new Dictionary<string, Type> { { "k", typeof(long) }, { "i", typeof(int) } }));

            Assert.AreEqual(null,
                           ReflectionHelper.GetMethodByName(typeof(GetMethodTestClass), BindingFlags.Instance | BindingFlags.NonPublic, "PrivateVoidMethodWithNoParameters", new Dictionary<string, Type> { { "i", typeof(int) } }));

        }

        [Test]
        public void CreatePropertyAccessItemReadonlyProperty()
        {
            ReflectionHelper.PropertyAccessItem propertyAccessItem = ReflectionHelper.CreatePropertyAccessItem(typeof(PropertyTestClass), typeof(PropertyTestClass).GetProperty("ReadOnlyProperty"));

            Assert.AreEqual(true, propertyAccessItem.CanRead);
            Assert.AreEqual(false, propertyAccessItem.CanWrite);

            Assert.NotNull(propertyAccessItem.Getter);
            Assert.IsNull(propertyAccessItem.Setter);
            Assert.AreEqual(1, propertyAccessItem.Getter(new PropertyTestClass()));
        }

        [Test]
        public void CreatePropertyAccessItemReadWriteProperty()
        {
            ReflectionHelper.PropertyAccessItem propertyAccessItem = ReflectionHelper.CreatePropertyAccessItem(typeof(PropertyTestClass), typeof(PropertyTestClass).GetProperty("ReadWriteProperty"));

            Assert.AreEqual(true, propertyAccessItem.CanRead);
            Assert.AreEqual(true, propertyAccessItem.CanWrite);

            Assert.NotNull(propertyAccessItem.Getter);
            Assert.NotNull(propertyAccessItem.Setter);

            PropertyTestClass propertyTestClass = new PropertyTestClass();
            Assert.AreEqual(1, propertyAccessItem.Getter(propertyTestClass));

            propertyAccessItem.Setter(propertyTestClass, 2);

            Assert.AreEqual(2, propertyAccessItem.Getter(propertyTestClass));
        }

        [Test]
        public void CreatePropertyAccessItemWriteOnlyProperty()
        {
            ReflectionHelper.PropertyAccessItem propertyAccessItem = ReflectionHelper.CreatePropertyAccessItem(typeof(PropertyTestClass), typeof(PropertyTestClass).GetProperty("WriteOnlyProperty"));

            Assert.AreEqual(false, propertyAccessItem.CanRead);
            Assert.AreEqual(true, propertyAccessItem.CanWrite);

            Assert.IsNull(propertyAccessItem.Getter);
            Assert.NotNull(propertyAccessItem.Setter);

            PropertyTestClass propertyTestClass = new PropertyTestClass();

            propertyAccessItem.Setter(propertyTestClass, 2);

            Assert.AreEqual(2, propertyTestClass.ReadWriteProperty);
        }

        [Test]
        public void SetPropertyValue()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            propertyTestClass.ReadWriteProperty = 1;

            ReflectionHelper.SetPropertyValue(propertyTestClass, "ReadWriteProperty", 2);

            Assert.AreEqual(2, propertyTestClass.ReadWriteProperty);
        }

        [Test]
        public void GetPropertyValue()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            propertyTestClass.ReadWriteProperty = 2;

            Assert.AreEqual(2, ReflectionHelper.GetPropertyValue(propertyTestClass, "ReadWriteProperty"));
        }

        [Test, ExpectedException(typeof(ReflectionHelperException))]
        public void GetPropertyValueThrowExceptionWhenNoGetMethod()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            ReflectionHelper.GetPropertyValue(propertyTestClass, "WriteOnlyProperty");
        }

        [Test, ExpectedException(typeof(ReflectionHelperException))]
        public void SetPropertyValueThrowExceptionWhenNoSetMethod()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            ReflectionHelper.SetPropertyValue(propertyTestClass, "ReadOnlyProperty", 1);
        }

        [Test, ExpectedException(typeof(ReflectionHelperException))]
        public void SetPropertyValueThrowsExceptionWhenNonImplicitlyConvertableValueIsSet()
        {
            PropertyTestClass propertyTestClass = new PropertyTestClass();
            ReflectionHelper.SetPropertyValue(propertyTestClass, "WriteOnlyProperty", 1F);
        }

        [Test]
        public void CallMethod()
        {
            CallMethodTestClass testClass = new CallMethodTestClass();
            Assert.AreEqual(testClass.PublicMethodWithNoParameters(), ReflectionHelper.CallMethod(testClass, "PublicMethodWithNoParameters"));
            Assert.AreEqual(testClass.PublicMethodWithParameters(5), ReflectionHelper.CallMethod(testClass, "PublicMethodWithParameters", 5));
            Assert.AreEqual(testClass.PublicMethodWithParameters(5, 20L), ReflectionHelper.CallMethod(testClass, "PublicMethodWithParameters", 5, 20L));
        }
    }
}
