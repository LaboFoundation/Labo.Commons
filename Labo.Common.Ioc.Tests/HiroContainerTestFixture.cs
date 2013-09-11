namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Hiro;

    using NUnit.Framework;

    [TestFixture]
    public class HiroContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new HiroIocContainer();
        }
    }
}