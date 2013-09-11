namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.TinyIoc;

    using NUnit.Framework;

    [TestFixture]
    public class TinyIocContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new TinyIocContainer();
        }
    }
}