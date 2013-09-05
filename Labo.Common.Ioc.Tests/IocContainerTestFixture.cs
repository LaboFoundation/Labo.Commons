namespace Labo.Common.Ioc.Tests
{
    using System;

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
        public void RegisterSingleInstance()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstance<ITestService>(x => new TestServiceImplementation());

            ITestService testService = iocContainer.GetInstance<ITestService>();
            Assert.IsNotNull(testService);
            
            ITestService secondServiceInstance = iocContainer.GetInstance<ITestService>();
            Assert.AreSame(testService, secondServiceInstance);
            Assert.AreEqual(testService.Guid, secondServiceInstance.Guid);
        }
    }
}
