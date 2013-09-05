﻿namespace Labo.Common.Ioc.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public abstract class IocContainerTestFixture
    {
        private interface ITestService
        {
            Guid Guid { get; set; }
        }

        private class TestServiceImplementation : ITestService
        {
            public Guid Guid { get; set; }

            public TestServiceImplementation()
            {
                Guid = Guid.NewGuid();
            }
        }

        public abstract IIocContainer CreateContainer();

        [Test]
        public void RegisterSingleInstanceLambda()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstance<ITestService>(x => new TestServiceImplementation());

            AssertRegisterSingleInstance<ITestService>(iocContainer);
        }

        private static void AssertRegisterSingleInstance<TService>(IIocContainerResolver iocContainer)
            where TService : ITestService
        {
            ITestService testService = iocContainer.GetInstance<TService>();
            Assert.IsNotNull(testService);

            ITestService secondServiceInstance = iocContainer.GetInstance<TService>();
            Assert.AreSame(testService, secondServiceInstance);
            Assert.AreEqual(testService.Guid, secondServiceInstance.Guid);
        }

        [Test]
        public void RegisterSingleInstanceGeneric()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstance<ITestService, TestServiceImplementation>();

            AssertRegisterSingleInstance<ITestService>(iocContainer);
        }

        [Test]
        public void RegisterSingleInstance()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstance(typeof(TestServiceImplementation));

            AssertRegisterSingleInstance<TestServiceImplementation>(iocContainer);
        }

        [Test]
        public void RegisterSingleInstanceNamedLambda()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstanceNamed<ITestService>(x => new TestServiceImplementation(), "TestService1");
            iocContainer.RegisterSingleInstanceNamed<ITestService>(x => new TestServiceImplementation(), "TestService2");

            AssertRegisterSingleInstanceNamed<ITestService>(iocContainer);
        }

        private static void AssertRegisterSingleInstanceNamed<TService>(IIocContainerResolver iocContainer)
            where TService : ITestService
        {
            TService[] allInstances = iocContainer.GetAllInstances<TService>().ToArray();
            Assert.IsNotNull(allInstances);
            Assert.AreEqual(2, allInstances.Length);

            TService testService1 = iocContainer.GetInstance<TService>("TestService1");
            Assert.IsNotNull(testService1);

            TService secondTestService1 = iocContainer.GetInstance<TService>("TestService1");
            Assert.AreSame(testService1, secondTestService1);
            Assert.AreEqual(testService1.Guid, secondTestService1.Guid);

            TService testService2 = iocContainer.GetInstance<TService>("TestService2");
            Assert.AreNotSame(testService1, testService2);
            Assert.AreNotEqual(testService1.Guid, testService2.Guid);

            Assert.IsTrue(allInstances.Contains(testService1));
            Assert.IsTrue(allInstances.Contains(testService2));
        }

        [Test]
        public void RegisterSingleInstanceNamedGeneric()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstanceNamed<ITestService, TestServiceImplementation>("TestService1");
            iocContainer.RegisterSingleInstanceNamed<ITestService, TestServiceImplementation>("TestService2");

            AssertRegisterSingleInstanceNamed<ITestService>(iocContainer);
        }

        [Test]
        public void RegisterInstance()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstance(typeof(TestServiceImplementation));

            AssertRegisterInstance<TestServiceImplementation>(iocContainer);
        }

        [Test]
        public void RegisterInstanceGeneric()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstance<ITestService, TestServiceImplementation>();

            AssertRegisterInstance<ITestService>(iocContainer);
        }

        private static void AssertRegisterInstance<TService>(IIocContainerResolver iocContainer)
            where TService : ITestService
        {
            ITestService testService = iocContainer.GetInstance<TService>();
            Assert.IsNotNull(testService);

            ITestService secondServiceInstance = iocContainer.GetInstance<TService>();
            Assert.AreNotSame(testService, secondServiceInstance);
            Assert.AreNotEqual(testService.Guid, secondServiceInstance.Guid);
        }
    }
}
