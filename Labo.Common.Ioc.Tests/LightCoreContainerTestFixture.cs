namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.LightCore;

    using NUnit.Framework;

    [TestFixture]
    public class LightCoreContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new LightCoreIocContainer();
        }
    }
}