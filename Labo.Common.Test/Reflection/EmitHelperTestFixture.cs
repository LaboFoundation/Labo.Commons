namespace Labo.Common.Tests.Reflection
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Reflection;
    using System.Text;

    using Labo.Common.Reflection;

    using NUnit.Framework;

    [TestFixture]
    public class DynamicMethodHelperTestFixture
    {
        private class Person
        {
            public string Name { get; private set; }
            public string Surname { get; private set; }
            public int Age { get; private set; }
            public Child Child { get; private set; }

            public const string DEFAULT_NAME = "Patrik";
            public const string DEFAULT_SURNAME = "Sjöberg";
            public const int DEFAULT_AGE = 48;

            public Person()
                : this(DEFAULT_NAME, DEFAULT_SURNAME, DEFAULT_AGE)
            {
            }

            public Person(string name, string surname, int age)
                : this(name, surname, age, null)
            {
               
            }

            public Person(string name, string surname, int age, Child child)
            {
                Name = name;
                Surname = surname;
                Age = age;
                Child = child;
            }
        }

        private class Child
        {
            public string Name { get; private set; }

            public Child(string name)
            {
                Name = name;
            }
        }

        private enum TestEnum
        {
            Prop1 = 0,
            Prop2 = 1
        }

        [Test]
        public void EmitConstructorInvokerInvokeWithParametersPrimitiveTypes()
        {
            Type personType = typeof(Person);
            Type[] parameterTypes = { typeof(string), typeof(string), typeof(int) };
            ConstructorInvoker constructorInvoker = DynamicMethodHelper.EmitConstructorInvoker(
                personType,
                personType.GetConstructor(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    parameterTypes,
                    null),
                parameterTypes);

            const string name = "javier";
            const string surname = "sotomayor";
            const int age = 45;

            Person person = (Person)constructorInvoker(name, surname, age);

            Assert.IsNotNull(person);
            Assert.AreEqual(name, person.Name);
            Assert.AreEqual(surname, person.Surname);
            Assert.AreEqual(age, person.Age);
        }

        [Test]
        public void EmitConstructorInvokerInvokeWithParametersComplexTypes()
        {
            Type personType = typeof(Person);
            Type[] parameterTypes = { typeof(string), typeof(string), typeof(int), typeof(Child) };
            ConstructorInvoker constructorInvoker = DynamicMethodHelper.EmitConstructorInvoker(
                personType,
                personType.GetConstructor(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    parameterTypes,
                    null),
                parameterTypes);

            const string name = "javier";
            const string surname = "sotomayor";
            const int age = 45;
            const string childName = "child";

            Child child = new Child(childName);
            Person person = (Person)constructorInvoker(name, surname, age, child);

            Assert.IsNotNull(person);
            Assert.AreEqual(name, person.Name);
            Assert.AreEqual(surname, person.Surname);
            Assert.AreEqual(age, person.Age);

            Assert.IsNotNull(person.Child);
            Assert.AreSame(child, person.Child);
            Assert.AreEqual(childName, person.Child.Name);
        }

        [Test]
        public void EmitConstructorInvokerInvokeParameterlessConstructor()
        {
            Type personType = typeof(Person);
            Type[] parameterTypes = { };
            ConstructorInvoker constructorInvoker = DynamicMethodHelper.EmitConstructorInvoker(
                personType,
                personType.GetConstructor(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    parameterTypes,
                    null),
                parameterTypes);

            Person person = (Person)constructorInvoker();

            Assert.IsNotNull(person);
            Assert.AreEqual(Person.DEFAULT_NAME, person.Name);
            Assert.AreEqual(Person.DEFAULT_SURNAME, person.Surname);
            Assert.AreEqual(Person.DEFAULT_AGE, person.Age);
        }

        [Test]
        public void EmitConstructorInvokerConstructStructValues()
        {
            Assert.AreEqual(default(bool), DynamicMethodHelper.EmitConstructorInvoker(typeof(bool))());
            Assert.AreEqual(default(byte), DynamicMethodHelper.EmitConstructorInvoker(typeof(byte))());
            Assert.AreEqual(default(sbyte), DynamicMethodHelper.EmitConstructorInvoker(typeof(sbyte))());
            Assert.AreEqual(default(short), DynamicMethodHelper.EmitConstructorInvoker(typeof(short))());
            Assert.AreEqual(default(ushort), DynamicMethodHelper.EmitConstructorInvoker(typeof(ushort))());
            Assert.AreEqual(default(int), DynamicMethodHelper.EmitConstructorInvoker(typeof(int))());
            Assert.AreEqual(default(uint), DynamicMethodHelper.EmitConstructorInvoker(typeof(uint))());
            Assert.AreEqual(default(long), DynamicMethodHelper.EmitConstructorInvoker(typeof(long))());
            Assert.AreEqual(default(ulong), DynamicMethodHelper.EmitConstructorInvoker(typeof(ulong))());
            Assert.AreEqual(default(float), DynamicMethodHelper.EmitConstructorInvoker(typeof(float))());
            Assert.AreEqual(default(decimal), DynamicMethodHelper.EmitConstructorInvoker(typeof(decimal))());
            Assert.AreEqual(default(Rectangle), DynamicMethodHelper.EmitConstructorInvoker(typeof(Rectangle))());
            Assert.AreEqual(default(Point), DynamicMethodHelper.EmitConstructorInvoker(typeof(Point))());
            Assert.AreEqual(default(Color), DynamicMethodHelper.EmitConstructorInvoker(typeof(Color))());
            Assert.AreEqual(default(DateTime), DynamicMethodHelper.EmitConstructorInvoker(typeof(DateTime))());
        }

        [Test]
        public void EmitConstructorInvokerConstructStructValuesWithParameters()
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            Type[] parameterTypes = { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) };
            object value = DynamicMethodHelper.EmitConstructorInvoker(typeof(DateTime), typeof(DateTime).GetConstructor(bindingFlags, null, parameterTypes, null), parameterTypes)(2013, 9, 10, 16, 40, 30);
            Assert.AreEqual(new DateTime(2013, 9, 10, 16, 40, 30), value);
            
            parameterTypes = new [] { typeof(int), typeof(int), typeof(int), typeof(int) };
            value = DynamicMethodHelper.EmitConstructorInvoker(typeof(Rectangle), typeof(Rectangle).GetConstructor(bindingFlags, null, parameterTypes, null), parameterTypes)(40, 20, 300, 200);
            Assert.AreEqual(new Rectangle(40, 20, 300, 200), value);

            parameterTypes = new [] { typeof(int), typeof(int) };
            value = DynamicMethodHelper.EmitConstructorInvoker(typeof(Point), typeof(Point).GetConstructor(bindingFlags, null, parameterTypes, null), parameterTypes)(10, 20);
            Assert.AreEqual(new Point(10, 20), value);
        }

        [Test]
        public void EmitConstructorInvokerConstructNullableValues()
        {
            Assert.AreEqual(default(bool?), DynamicMethodHelper.EmitConstructorInvoker(typeof(bool?))());
            Assert.AreEqual(default(byte?), DynamicMethodHelper.EmitConstructorInvoker(typeof(byte?))());
            Assert.AreEqual(default(sbyte?), DynamicMethodHelper.EmitConstructorInvoker(typeof(sbyte?))());
            Assert.AreEqual(default(short?), DynamicMethodHelper.EmitConstructorInvoker(typeof(short?))());
            Assert.AreEqual(default(ushort?), DynamicMethodHelper.EmitConstructorInvoker(typeof(ushort?))());
            Assert.AreEqual(default(int?), DynamicMethodHelper.EmitConstructorInvoker(typeof(int?))());
            Assert.AreEqual(default(uint?), DynamicMethodHelper.EmitConstructorInvoker(typeof(uint?))());
            Assert.AreEqual(default(long?), DynamicMethodHelper.EmitConstructorInvoker(typeof(long?))());
            Assert.AreEqual(default(ulong?), DynamicMethodHelper.EmitConstructorInvoker(typeof(ulong?))());
            Assert.AreEqual(default(float?), DynamicMethodHelper.EmitConstructorInvoker(typeof(float?))());
            Assert.AreEqual(default(decimal?), DynamicMethodHelper.EmitConstructorInvoker(typeof(decimal?))());
            Assert.AreEqual(default(DateTime?), DynamicMethodHelper.EmitConstructorInvoker(typeof(DateTime?))());
        }

        [Test]
        public void EmitConstructorInvokerConstructEnum()
        {
            TestEnum enumValue = (TestEnum)DynamicMethodHelper.EmitConstructorInvoker(typeof(TestEnum))();
            Assert.AreEqual(default(TestEnum), enumValue);
        }

        private class MathHelper
        {
            public double PI { get; private set; }

            public int Sum(int a, int b)
            {
                return a + b;
            }

            public virtual int GetAnInteger()
            {
                return 10;
            }

            public void SetPI(double pi)
            {
                PI = pi;
            }

            public double GetPI()
            {
                return PI;
            }
        }

        private class MathHelperImpl : MathHelper
        {
            public override int GetAnInteger()
            {
                return 20;
            }
        }

        [Test]
        public void EmitMethodInvokerInvokeMethodWithReturnType()
        {
            Type[] parameterTypes = { typeof(int), typeof(int) };
            MathHelper mathHelper = new MathHelper();
            object value = DynamicMethodHelper.EmitMethodInvoker(typeof(MathHelper), typeof(MathHelper).GetMethod("Sum", parameterTypes))(mathHelper, 10, 5);

            Assert.AreEqual(mathHelper.Sum(10, 5), (int)value);
        }

        [Test]
        public void EmitMethodInvokerInvokeMethodWithReturnTypeAndNoArguments()
        {
            MathHelper mathHelper = new MathHelper();
            mathHelper.SetPI(Math.PI);
            object value = DynamicMethodHelper.EmitMethodInvoker(
                typeof(MathHelper),
                typeof(MathHelper).GetMethod("GetPI"))(mathHelper);

            Assert.AreEqual(Math.PI, (double)value);
        }

        [Test]
        public void EmitMethodInvokerInvokeVirtualMethodWithReturnTypeAndNoArguments()
        {
            MathHelper mathHelper = new MathHelper();
            object value = DynamicMethodHelper.EmitMethodInvoker(
                typeof(MathHelper),
                typeof(MathHelper).GetMethod("GetAnInteger"))(mathHelper);

            Assert.AreEqual(10, (int)value);

            MathHelperImpl mathHelperImpl = new MathHelperImpl();
            value = DynamicMethodHelper.EmitMethodInvoker(
                typeof(MathHelperImpl),
                typeof(MathHelperImpl).GetMethod("GetAnInteger"))(mathHelperImpl);

            Assert.AreEqual(20, (int)value);
        }

        [Test]
        public void EmitMethodInvokerInvokeMethodWithoutReturnType()
        {
            MathHelper mathHelper = new MathHelper();
            DynamicMethodHelper.EmitMethodInvoker(
            typeof(MathHelper),
            typeof(MathHelper).GetMethod("SetPI", new[] { typeof(double) }))(mathHelper, Math.PI);

            Assert.AreEqual(Math.PI, mathHelper.PI);
        }
    }
}
