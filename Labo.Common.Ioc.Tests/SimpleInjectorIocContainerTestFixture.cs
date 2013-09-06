namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.SimpleInjector;

    using NUnit.Framework;

    [TestFixture]
    public class SimpleInjectorIocContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new SimpleInjectorIocContainer();
        }
    }
}
