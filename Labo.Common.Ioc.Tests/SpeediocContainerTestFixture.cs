namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Speedioc;

    using NUnit.Framework;

    [TestFixture]
    public class SpeediocContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new SpeediocIocContainer();
        }
    }
}