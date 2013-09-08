namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Griffin;

    using NUnit.Framework;

    [TestFixture]
    public class GriffinContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new GriffinIocContainer();
        }
    }
}
