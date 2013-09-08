namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.HaveBox;

    using NUnit.Framework;

    [TestFixture]
    public class HaveBoxContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new HaveBoxIocContainer();
        }
    }
}