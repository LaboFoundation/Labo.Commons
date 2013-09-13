namespace Labo.Common.Ioc.Tests.Performance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Labo.Common.Ioc.Autofac;
    using Labo.Common.Ioc.Dynamo;
    using Labo.Common.Ioc.HaveBox;
    using Labo.Common.Ioc.Hiro;
    using Labo.Common.Ioc.LightCore;
    using Labo.Common.Ioc.LightInject;
    using Labo.Common.Ioc.Linfu;
    using Labo.Common.Ioc.Mugen;
    using Labo.Common.Ioc.Munq;
    using Labo.Common.Ioc.NInject;
    using Labo.Common.Ioc.SimpleInjector;
    using Labo.Common.Ioc.StructureMap;
    using Labo.Common.Ioc.Tests.Performance.Domain;
    using Labo.Common.Ioc.TinyIoc;
    using Labo.Common.Ioc.Unity;

    using NUnit.Framework;

    [TestFixture]
    public class IocContainerPerformanceTest
    {
        private static readonly List<long> s_BatchIterations = new List<long> { 1000, 5000, 20000, 100000, 250000, 1000000, 5000000 };
        private static readonly Dictionary<string, Func<IIocContainer>> s_Containers = new Dictionary<string, Func<IIocContainer>>
                                                                                     {
                                                                                         { "NInject", () => new NInjectIocContainer() },
                                                                                         { "Linfu", () => new LinfuIocContainer() },
                                                                                         { "Unity", () => new UnityIocContainer() },
                                                                                         { "Autofac", () => new AutofacIocContainer() },
                                                                                         { "Mugen", () => new MugenIocContainer() },
                                                                                         { "TinyIoc", () => new TinyIocContainer() },
                                                                                         { "LightCore", () => new LightCoreIocContainer() },
                                                                                         { "Dynamo", () => new DynamoIocContainer() },
                                                                                         { "Hiro", () => new HiroIocContainer() },
                                                                                         { "Munq", () => new MunqIocContainer() },
                                                                                         { "LightInject", () => new LightInjectIocContainer() },
                                                                                         //{ "Structuremap", () => new StructureMapIocContainer() },
                                                                                         { "SimpleInjector", () => new SimpleInjectorIocContainer()},
                                                                                         { "Labo", () => new LaboIocContainer() },
                                                                                         { "Havebox", () => new HaveBoxIocContainer() },
                                                                                        
                                                                                     };
            
        [Test]
        public void TestPerformance()
        {
            Console.Write(string.Empty.PadRight(19, ' '));

            s_BatchIterations.ForEach(x => Console.Write(x.ToStringInvariant().PadRight(20, ' ')));

            foreach (KeyValuePair<string, Func<IIocContainer>> containerEntry in s_Containers)
            {
                IIocContainer container = containerEntry.Value();

                container.RegisterSingleInstance<ILogger, Logger>();
                container.RegisterSingleInstance<IErrorHandler, ErrorHandler>();

                Console.Write(string.Format("\n{0}", containerEntry.Key).PadRight(20, ' '));

                s_BatchIterations.ForEach(
                    x =>
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();

                            //warm up
                            container.GetInstance<IErrorHandler>();

                            Console.Write(MeasurePerformance(() => container.GetInstance<IErrorHandler>(), x).ToStringInvariant().PadRight(20, ' '));
                        });
            }
        }

        private static string MeasurePerformance(Action action, decimal iterations)
        {
            GC.Collect();
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < iterations; i++)
            {
                action();
            }

            stopwatch.Stop();

            long performance = stopwatch.ElapsedTicks;

            return string.Format("{0} ({1})", performance.ToStringInvariant(), (performance / iterations).ToStringInvariant());
        }
    }
}
