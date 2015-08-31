namespace Labo.Common.Tests.Reflection
{
    using System;
    using System.Reflection.Emit;
    using Labo.Common.Reflection;
    using NUnit.Framework;

    [TestFixture]
    public class DynamicMethodCacheTestFixture
    {
        [Test]
        public void Get()
        {
            DynamicMethodCache dynamicMethodCache = new DynamicMethodCache();
            MethodInvoker methodInvoker = (o, parameters) => null;
            Func<MethodInvoker> creatorFunc = () => methodInvoker;
            MethodInvoker cachedMethodInvoker = dynamicMethodCache.GetOrAddDelegate(
                // null,
                new DynamicMethodInfo(), 
                creatorFunc,
                DynamicMethodCacheStrategy.Temporary);

            creatorFunc = null;
            cachedMethodInvoker.ToStringInvariant();
        }
    }
}
