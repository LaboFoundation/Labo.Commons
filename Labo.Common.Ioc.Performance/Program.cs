namespace Labo.Common.Ioc.Performance
{
    using System;

    using Labo.Common.Ioc.Performance.Domain;
    using Labo.Common.Ioc.Registration;
    using Labo.Common.Utils;

    class Program
    {
        static void Main(string[] args)
        {
            LaboIocContainer container = new LaboIocContainer();

            container.RegisterInstance<ISettings, Settings>();
            container.RegisterInstance<ILogger, Logger>();
            container.RegisterInstance<IErrorHandler, ErrorHandler>();
            container.RegisterInstance<IApplication, Application>();
            container.RegisterInstance<IConfigurationManager, ConfigurationManager>();
            container.RegisterInstance<IController, Controller>();

            RegistrationBuilder registrationBuilder = new RegistrationBuilder();
            IApplication application = (IApplication)registrationBuilder.BuildRegistration(container, container.ModuleBuilder, typeof(Application), LaboIocServiceLifetime.Transient)();
            application.ToStringInvariant();

            for (int i = 0; i < 10000; i++)
            {
                IErrorHandler errorHandler = container.GetInstance<IErrorHandler>();
                errorHandler.Logger.Log(new Exception());
            }
        }
    }

    public static class ServiceInvoker
    {
        private static readonly ILogger s_Logger = new Logger();
        private static readonly IConfigurationManager s_ConfigurationManager = new ConfigurationManager();
        private static readonly IErrorHandler s_ErrorHandler = new ErrorHandler(s_Logger, new Settings(s_ConfigurationManager, s_Integer));

        private static readonly DateTime s_DateTime = default(DateTime);

        private static readonly DateTime s_Integer = default(DateTime);

        private static readonly short s_Short = default(short);

        private static readonly ILogger s_NullLogger = default(ILogger);

        public static IApplication CreateInstance()
        {
            return new Application(new Controller(s_ErrorHandler));
        }

        public static IErrorHandler CreateSingleInstance()
        {
            return s_ErrorHandler;
        }

        public static IApplication CreateTransientInstance()
        {
            return new Application(new Controller(new ErrorHandler(new Logger(), new Settings(new ConfigurationManager(), default(DateTime), default(DateTime)))));
        }
    }

    public static class ServiceInvoker1
    {
        private static readonly ILogger s_Logger = new Logger();
        private static readonly IConfigurationManager s_ConfigurationManager = new ConfigurationManager();
        private static readonly ISettings s_Settings = new Settings(s_ConfigurationManager, default(DateTime));
        private static readonly IErrorHandler s_ErrorHandler = new ErrorHandler(s_Logger, s_Settings);
        private static readonly IController s_Controller = new Controller(s_ErrorHandler);
        private static readonly IApplication s_Application = new Application(s_Controller);


        public static IApplication CreateInstance()
        {
            return s_Application;
        }

        public static IErrorHandler CreateSingleInstance()
        {
            return s_ErrorHandler;
        }
    }
}
