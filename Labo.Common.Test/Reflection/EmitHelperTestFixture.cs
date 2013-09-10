namespace Labo.Common.Tests.Reflection
{
    using System;
    using System.Drawing;
    using System.Reflection;

    using Labo.Common.Reflection;

    using NUnit.Framework;

    [TestFixture]
    public class EmitHelperTestFixture
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
            ConstructorInvoker constructorInvoker = EmitHelper.EmitConstructorInvoker(
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
            ConstructorInvoker constructorInvoker = EmitHelper.EmitConstructorInvoker(
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
            ConstructorInvoker constructorInvoker = EmitHelper.EmitConstructorInvoker(
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
            Assert.AreEqual(default(bool), EmitHelper.EmitConstructorInvoker(typeof(bool))());
            Assert.AreEqual(default(byte), EmitHelper.EmitConstructorInvoker(typeof(byte))());
            Assert.AreEqual(default(sbyte), EmitHelper.EmitConstructorInvoker(typeof(sbyte))());
            Assert.AreEqual(default(short), EmitHelper.EmitConstructorInvoker(typeof(short))());
            Assert.AreEqual(default(ushort), EmitHelper.EmitConstructorInvoker(typeof(ushort))());
            Assert.AreEqual(default(int), EmitHelper.EmitConstructorInvoker(typeof(int))());
            Assert.AreEqual(default(uint), EmitHelper.EmitConstructorInvoker(typeof(uint))());
            Assert.AreEqual(default(long), EmitHelper.EmitConstructorInvoker(typeof(long))());
            Assert.AreEqual(default(ulong), EmitHelper.EmitConstructorInvoker(typeof(ulong))());
            Assert.AreEqual(default(float), EmitHelper.EmitConstructorInvoker(typeof(float))());
            Assert.AreEqual(default(decimal), EmitHelper.EmitConstructorInvoker(typeof(decimal))());
            Assert.AreEqual(default(Rectangle), EmitHelper.EmitConstructorInvoker(typeof(Rectangle))());
            Assert.AreEqual(default(Point), EmitHelper.EmitConstructorInvoker(typeof(Point))());
            Assert.AreEqual(default(Color), EmitHelper.EmitConstructorInvoker(typeof(Color))());
            Assert.AreEqual(default(DateTime), EmitHelper.EmitConstructorInvoker(typeof(DateTime))());
        }

        [Test]
        public void EmitConstructorInvokerConstructStructValuesWithParameters()
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            Type[] parameterTypes = { typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int) };
            object value = EmitHelper.EmitConstructorInvoker(typeof(DateTime), typeof(DateTime).GetConstructor(bindingFlags, null, parameterTypes, null), parameterTypes)(2013, 9, 10, 16, 40, 30);
            Assert.AreEqual(new DateTime(2013, 9, 10, 16, 40, 30), value);
            
            parameterTypes = new [] { typeof(int), typeof(int), typeof(int), typeof(int) };
            value = EmitHelper.EmitConstructorInvoker(typeof(Rectangle), typeof(Rectangle).GetConstructor(bindingFlags, null, parameterTypes, null), parameterTypes)(40, 20, 300, 200);
            Assert.AreEqual(new Rectangle(40, 20, 300, 200), value);

            parameterTypes = new [] { typeof(int), typeof(int) };
            value = EmitHelper.EmitConstructorInvoker(typeof(Point), typeof(Point).GetConstructor(bindingFlags, null, parameterTypes, null), parameterTypes)(10, 20);
            Assert.AreEqual(new Point(10, 20), value);
        }

        [Test]
        public void EmitConstructorInvokerConstructNullableValues()
        {
            Assert.AreEqual(default(bool?), EmitHelper.EmitConstructorInvoker(typeof(bool?))());
            Assert.AreEqual(default(byte?), EmitHelper.EmitConstructorInvoker(typeof(byte?))());
            Assert.AreEqual(default(sbyte?), EmitHelper.EmitConstructorInvoker(typeof(sbyte?))());
            Assert.AreEqual(default(short?), EmitHelper.EmitConstructorInvoker(typeof(short?))());
            Assert.AreEqual(default(ushort?), EmitHelper.EmitConstructorInvoker(typeof(ushort?))());
            Assert.AreEqual(default(int?), EmitHelper.EmitConstructorInvoker(typeof(int?))());
            Assert.AreEqual(default(uint?), EmitHelper.EmitConstructorInvoker(typeof(uint?))());
            Assert.AreEqual(default(long?), EmitHelper.EmitConstructorInvoker(typeof(long?))());
            Assert.AreEqual(default(ulong?), EmitHelper.EmitConstructorInvoker(typeof(ulong?))());
            Assert.AreEqual(default(float?), EmitHelper.EmitConstructorInvoker(typeof(float?))());
            Assert.AreEqual(default(decimal?), EmitHelper.EmitConstructorInvoker(typeof(decimal?))());
            Assert.AreEqual(default(DateTime?), EmitHelper.EmitConstructorInvoker(typeof(DateTime?))());
        }

        [Test]
        public void EmitConstructorInvokerConstructEnum()
        {
            TestEnum enumValue = (TestEnum)EmitHelper.EmitConstructorInvoker(typeof(TestEnum))();
            Assert.AreEqual(default(TestEnum), enumValue);
        }
    }
}
