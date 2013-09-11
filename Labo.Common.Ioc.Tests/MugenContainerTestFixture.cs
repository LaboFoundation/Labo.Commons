namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Mugen;

    using NUnit.Framework;

    [TestFixture]
    public class MugenContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new MugenIocContainer();
        }
    }
}