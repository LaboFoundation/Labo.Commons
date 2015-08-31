namespace Labo.Common.Reflection.Benchmark
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    using Labo.Common.Expression;

    class Program
    {
        private class Person
        {
            internal static int counter;
            internal string name;

            internal Person()
            {
            }

            internal Person(int age, string name)
            {
                Age = age;
                this.name = name;
            }

            public object this[int a, int b]
            {
                get { return null; }
                set { }
            }

            internal int Age { get; set; }
            internal static int Counter { get; set; }

            internal void Walk()
            {
            }

            internal void Walk(int speed)
            {
            }

            internal static void Generate()
            {
            }

            internal static void Generate(int seed)
            {
            }

            internal string GetName()
            {
                return name;
            }

            internal string GetName(string prefix)
            {
                return prefix + " " + name;
            }
        }

        private static readonly int[] s_Iterations = { 20000, 2000000 };

        static void Main(string[] args)
        {
            RunConstructorBenchmark();
            RunMethodBenchmark();

            Console.ReadLine();
        }

        private static void RunMethodBenchmark()
        {
            MethodInfo methodInfo = typeof(Person).GetMethod("GetName", BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            MethodInvoker methodInvoker = DynamicMethodHelper.EmitMethodInvoker(typeof(Person), methodInfo);
            object[] emptyParameters = new object[0];

            Dictionary<string, Action> actions = new Dictionary<string, Action>
                                                     {
                                                         { "Direct method", () => new Person().GetName() },
                                                         { "Reflection method", () => methodInfo.Invoke(new Person(), emptyParameters) },
                                                         { "dynamic method", () =>
                                                                                 {
                                                                                     dynamic person = new Person();
                                                                                     string name = person.GetName();
                                                                                 } 
                                                         },
                                                         { "Labo expression method", () => ExpressionHelper.CallMethod(new Person(), methodInfo) },
                                                         { "Labo method", () => ReflectionHelper.CallMethod(new Person(), methodInfo) },
                                                         { "Labo cached method", () => methodInvoker(new Person()) },
                                                     };
            Execute("Benchmark for Method Invocation", actions);
        }

        private static void RunConstructorBenchmark()
        {
            ConstructorInfo constructorInfo = typeof(Person).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new Type[0],
                null);
            ConstructorInvoker constructorInvoker = DynamicMethodHelper.EmitConstructorInvoker(typeof(Person));
            object[] emptyParameters = new object[0];

            Dictionary<string, Action> actions = new Dictionary<string, Action>
                                                     {
                                                         { "Direct ctor", () => new Person() },
                                                         { "Reflection ctor", () => constructorInfo.Invoke(emptyParameters) },
                                                         { "Labo ctor", () => ReflectionHelper.CreateInstance(typeof(Person), constructorInfo, Type.EmptyTypes) },
                                                         { "Labo cached ctor", () => constructorInvoker() },
                                                     };
            Execute("Benchmark for Object Construction", actions);
        }

        private static void Execute(string name, IEnumerable<KeyValuePair<string, Action>> actions)
        {
            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("!!!! {0}", name);

            for (int i = 0; i < s_Iterations.Length; i++)
            {
                int iterationCount = s_Iterations[i];
                Console.WriteLine("||Executing for {0} iterations|| ||", iterationCount);
                Measure(stopwatch, actions, iterationCount);
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private static void Measure(Stopwatch watch, IEnumerable<KeyValuePair<string, Action>> actions, int iterationCount)
        {
            IEnumerator<KeyValuePair<string, Action>> enumerator = actions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, Action> entry = enumerator.Current;

                watch.Start();
                
                for (int i = 0; i < iterationCount; i++)
                {
                    entry.Value();
                }

                watch.Stop();
                Console.WriteLine("|{0,-35} | {1,6} ms|", entry.Key + ":", watch.ElapsedMilliseconds);
                watch.Reset();
            }
        }
    }
}
