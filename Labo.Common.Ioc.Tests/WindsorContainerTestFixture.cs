namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Windsor;

    using NUnit.Framework;

    [TestFixture]
    public class WindsorContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new WindsorIocContainer();
        }
    }
}