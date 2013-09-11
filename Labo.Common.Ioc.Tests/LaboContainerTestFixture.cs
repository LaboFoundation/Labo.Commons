namespace Labo.Common.Ioc.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class LaboContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new LaboIocContainer();
        }
    }
}