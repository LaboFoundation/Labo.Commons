﻿namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Petite;

    using NUnit.Framework;

    [TestFixture]
    public class PetiteContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new PetiteIocContainer();
        }
    }
}