namespace Labo.Common.Ioc.Performance
{
    using System;

    using Labo.Common.Ioc.Performance.Domain;

    class Program
    {
        static void Main(string[] args)
        {
            IIocContainer container = new LaboIocContainer();

            container.RegisterSingleInstance<ILogger, Logger>();
            container.RegisterSingleInstance<IErrorHandler, ErrorHandler>();

            for (int i = 0; i < 10000; i++)
            {
                IErrorHandler errorHandler = container.GetInstance<IErrorHandler>();
                errorHandler.Logger.Log(new Exception());
            }
        }
    }
}
