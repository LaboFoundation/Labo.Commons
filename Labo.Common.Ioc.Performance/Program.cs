namespace Labo.Common.Ioc.Performance
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Ioc.Performance.Domain;
    using Labo.Common.Ioc.Registration;
    using Labo.Common.Utils;

    class Program
    {
        static void Main(string[] args)
        {
            AssemblyBuilder dynamicAssembly =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    new AssemblyName("Labo.Common.Ioc.Container.Compiled"),
                    AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder =
                dynamicAssembly.DefineDynamicModule("Labo.Common.Ioc.DynamicModule");
            
            ClassGenerator classGenerator = new ClassGenerator(moduleBuilder, "InstanceCreator", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);
            FieldGenerator loggerField = new FieldGenerator(classGenerator, "fld1", new InstanceGenerator(typeof(Logger), typeof(Logger).GetConstructors().FirstOrDefault()), FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);
            classGenerator.AddField(loggerField);
            CastInstanceGenerator loggerInstanceGenerator = new CastInstanceGenerator(typeof(ILogger), loggerField);
            CastInstanceGenerator configurationManagerInstanceGenerator =
                new CastInstanceGenerator(
                    typeof(IConfigurationManager),
                    new InstanceGenerator(
                        typeof(ConfigurationManager),
                        typeof(ConfigurationManager).GetConstructors().FirstOrDefault()));
            CastInstanceGenerator settingsInstanceGenerator = new CastInstanceGenerator(
                typeof(ISettings),
                new InstanceGenerator(
                    typeof(Settings),
                    typeof(Settings).GetConstructors().FirstOrDefault(),
                    configurationManagerInstanceGenerator));
            CastInstanceGenerator errorHandlerInstanceGenerator = new CastInstanceGenerator(
                typeof(IErrorHandler),
                new InstanceGenerator(
                    typeof(ErrorHandler),
                    typeof(ErrorHandler).GetConstructors().FirstOrDefault(),
                    loggerInstanceGenerator,
                    settingsInstanceGenerator));
            CastInstanceGenerator controllerInstanceGenerator = new CastInstanceGenerator(
                typeof(IController),
                new InstanceGenerator(
                    typeof(Controller),
                    typeof(Controller).GetConstructors().FirstOrDefault(),
                    errorHandlerInstanceGenerator));
            IInstanceGenerator applicationInstanceGenerator = new CastInstanceGenerator(
                typeof(IApplication),
                new InstanceGenerator(
                    typeof(Application),
                    typeof(Application).GetConstructors().FirstOrDefault(),
                    controllerInstanceGenerator));

            MethodGenerator createInstanceMethod = new MethodGenerator(classGenerator, "CreateInstance", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig, applicationInstanceGenerator);
            classGenerator.AddMethod(createInstanceMethod);

            classGenerator.Generate();
            dynamicAssembly.Save("test.dll");

            Func<object> @delegate = (Func<object>)classGenerator.GetMethod<Func<object>>("CreateInstance");
            object instance = @delegate();
            instance.ToStringInvariant();

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
        private static readonly IErrorHandler s_ErrorHandler = new ErrorHandler(s_Logger, new Settings(s_ConfigurationManager));

        private static readonly DateTime s_DateTime = default(DateTime);

        private static readonly DateTime s_Integer = default(DateTime);

        private static readonly short s_Short = default(short);

        private static readonly ILogger s_NullLogger = default(ILogger);

        public static IApplication CreateInstance()
        {
            Application application = new Application(new Controller(s_ErrorHandler));
            return application;
        }

        public static IErrorHandler CreateSingleInstance()
        {
            IErrorHandler singleInstance = s_ErrorHandler;
            return singleInstance;
        }

        public static IApplication CreateTransientInstance()
        {
            return new Application(new Controller(new ErrorHandler(new Logger(), new Settings(new ConfigurationManager()))));
        }
    }

    public static class ServiceInvoker1
    {
        private static readonly ILogger s_Logger = new Logger();
        private static readonly IConfigurationManager s_ConfigurationManager = new ConfigurationManager();
        private static readonly ISettings s_Settings = new Settings(s_ConfigurationManager);
        private static readonly IErrorHandler s_ErrorHandler = new ErrorHandler(s_Logger, s_Settings);
        private static readonly IController s_Controller = new Controller(s_ErrorHandler);
        private static readonly IApplication s_Application = new Application(s_Controller);


        public static IApplication CreateInstance()
        {
            IApplication application = s_Application;
            return application;
        }

        public static IErrorHandler CreateSingleInstance()
        {
            return s_ErrorHandler;
        }
    }
}
