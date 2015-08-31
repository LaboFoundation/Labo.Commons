namespace Labo.Common.Benchmark
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    class Program
    {
        static void Main(string[] args)
        {
            var dynamicDictionaryList = new[]
                                            {
                                                new
                                                {
                                                    Id = 1,
                                                    Title = "Test Title",
                                                    Description = "Test Description",
                                                    Date = DateTime.Now
                                                },
                                                new
                                                {
                                                    Id = 2,
                                                    Title = "Test Title",
                                                    Description = "Test Description",
                                                    Date = DateTime.Now
                                                },
                                                new
                                                {
                                                    Id = 3,
                                                    Title = "Test Title",
                                                    Description = "Test Description",
                                                    Date = DateTime.Now
                                                }
                                            };

            var dictionaryList = new[]
                                    {
                                        new Dictionary<string, object>
                                        {
                                            { "Id", 1 },
                                            { "Title", "Test Title" },
                                            { "Description", "Test Description" },
                                            { "Date", DateTime.Now }
                                        },
                                        new Dictionary<string, object>
                                        {
                                            { "Id", 2 },
                                            { "Title", "Test Title" },
                                            { "Description", "Test Description" },
                                            { "Date", DateTime.Now }
                                        },
                                        new Dictionary<string, object>
                                        {
                                            { "Id", 3 },
                                            { "Title", "Test Title" },
                                            { "Description", "Test Description" },
                                            { "Date", DateTime.Now }
                                        },
                                    };

            object value;

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < 1000000; i++)
            {
                for (int j = 0; j < dynamicDictionaryList.Length; j++)
                {
                    var dynamicObject = dynamicDictionaryList[j];
                    value = dynamicObject.Id;
                    value = dynamicObject.Title;
                    value = dynamicObject.Description;
                    value = dynamicObject.Date;
                }
            }

            stopwatch.Stop();

            Console.WriteLine("Dynamic Dictionary Get: {0}", stopwatch.Elapsed);



            stopwatch.Restart();

            for (int i = 0; i < 1000000; i++)
            {
                for (int j = 0; j < dictionaryList.Length; j++)
                {
                    Dictionary<string, object> dynamicObject = dictionaryList[j];
                    value = dynamicObject["Id"];
                    value = dynamicObject["Title"];
                    value = dynamicObject["Description"];
                    value = dynamicObject["Date"];
                }
            }

            stopwatch.Stop();

            Console.WriteLine("Dictionary Get: {0}", stopwatch.Elapsed);

            Console.ReadLine();
        }
    }
}
