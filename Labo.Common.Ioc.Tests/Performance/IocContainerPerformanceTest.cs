namespace Labo.Common.Ioc.Tests.Performance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Funq;

    using Labo.Common.Ioc.Autofac;
    using Labo.Common.Ioc.SimpleInjector;
    using Labo.Common.Ioc.Tests.Performance.Domain;

    using NUnit.Framework;

    [TestFixture]
    public class IocContainerPerformanceTest
    {
        private static readonly List<long> s_BatchIterations = new List<long> { 1000, 5000, 20000, 100000, 250000, 1000000, 5000000 };
        private static readonly Dictionary<string, Func<IIocContainer>> s_Containers = new Dictionary<string, Func<IIocContainer>>
                                                                                     {
                                                                                         { "Autofac", () => new AutofacIocContainer() },
                                                                                         { "SimpleInjector", () => new SimpleInjectorIocContainer()},
                                                                                         { "Labo", () => new LaboIocContainer() },
                                                                                     };
            
        [Test]
        public void TestPerformance()
        {
            Console.Write(string.Empty.PadRight(19, ' '));

            s_BatchIterations.ForEach(x => Console.Write(x.ToStringInvariant().PadRight(20, ' ')));

            foreach (KeyValuePair<string, Func<IIocContainer>> containerEntry in s_Containers)
            {
                IIocContainer container = containerEntry.Value();

                container.RegisterSingleInstance<ILogger>(x => new Logger());
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

            Container funqContainer = new Container();
            funqContainer.Register<ILogger>(x => new Logger()).ReusedWithin(ReuseScope.Container);

            Console.Write("\nFunq".PadRight(20, ' '));

            s_BatchIterations.ForEach(
                x =>
                {
                    //warm up
                    funqContainer.Resolve<ILogger>();

                    GC.Collect();

                    Console.Write(MeasurePerformance(() =>  funqContainer.Resolve<ILogger>(), x).ToStringInvariant().PadRight(20, ' '));
                });
        }

        private static string MeasurePerformance(Action action, decimal iterations)
        {
            GC.Collect();
            long begin = Stopwatch.GetTimestamp();

            for (int i = 0; i < iterations; i++)
            {
                action();
            }

            long end = Stopwatch.GetTimestamp();

            long performance = end - begin;
            return string.Format("{0} ({1})", performance.ToStringInvariant(), (performance / iterations).ToStringInvariant());
        }
    }
}
