namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Catel;

    using NUnit.Framework;

    [TestFixture]
    public class CatelContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new CatelIocContainer();
        }
    }
}