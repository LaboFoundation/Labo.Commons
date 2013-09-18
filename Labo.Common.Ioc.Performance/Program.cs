namespace Labo.Common.Ioc.Performance
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Ioc.Performance.Domain;
    using Labo.Common.Ioc.Registration;

    class Program
    {
        static void Main(string[] args)
        {
            AssemblyBuilder dynamicAssembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Labo.Common.Ioc.Container.Compiled"), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = dynamicAssembly.DefineDynamicModule("Labo.Common.Ioc.DynamicModule");
            
            //ClassGenerator classGenerator = new ClassGenerator(moduleBuilder, "InstanceCreator", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);
            //const FieldAttributes privateStaticReadonly = FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly;
            //FieldGenerator loggerField = new FieldGenerator(classGenerator, "fld1", new InstanceGenerator(typeof(Logger), typeof(Logger).GetConstructors().FirstOrDefault()), privateStaticReadonly);
            //classGenerator.AddField(loggerField);
            //CastInstanceGenerator loggerInstanceGenerator = new CastInstanceGenerator(typeof(ILogger), new LoadFieldGenerator(loggerField));
            //CastInstanceGenerator configurationManagerInstanceGenerator = new InstanceGenerator(
            //            typeof(ConfigurationManager),
            //            typeof(ConfigurationManager).GetConstructors().FirstOrDefault()).Cast(typeof(IConfigurationManager));
            //CastInstanceGenerator settingsInstanceGenerator = new InstanceGenerator(
            //        typeof(Settings),
            //        typeof(Settings).GetConstructors().FirstOrDefault(),
            //        configurationManagerInstanceGenerator).Cast(typeof(ISettings));
            //CastInstanceGenerator errorHandlerInstanceGenerator =
            //    new InstanceGenerator(
            //        typeof(ErrorHandler),
            //        typeof(ErrorHandler).GetConstructors().FirstOrDefault(),
            //        loggerInstanceGenerator,
            //        settingsInstanceGenerator).Cast(typeof(IErrorHandler));

            //FieldGenerator errorHandlerField = new FieldGenerator(classGenerator, "fld2", errorHandlerInstanceGenerator, privateStaticReadonly);
            //classGenerator.AddField(errorHandlerField);

            //CastInstanceGenerator controllerInstanceGenerator = new InstanceGenerator(
            //        typeof(Controller),
            //        typeof(Controller).GetConstructors().FirstOrDefault(),
            //        new LoadFieldGenerator(errorHandlerField)).Cast(typeof(IController));
            //IInstanceGenerator applicationInstanceGenerator = new InstanceGenerator(
            //        typeof(Application),
            //        typeof(Application).GetConstructors().FirstOrDefault(),
            //        controllerInstanceGenerator).Cast(typeof(IApplication));
            
            //FieldGenerator applicationFieldGenerator = new FieldGenerator(classGenerator, "fld3", applicationInstanceGenerator, privateStaticReadonly);
            //classGenerator.AddField(applicationFieldGenerator);

            //MethodGenerator createInstanceMethod = new MethodGenerator(classGenerator, "CreateInstance", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig, new LoadFieldGenerator(applicationFieldGenerator));
            //classGenerator.AddMethod(createInstanceMethod);

            //classGenerator.Generate();

            //Func<object> @delegate = (Func<object>)classGenerator.GetMethod<Func<object>>("CreateInstance");
            //object instance = @delegate();
            //instance.ToStringInvariant();

            LaboIocContainer container = new LaboIocContainer();

            container.RegisterInstance<ISettings, Settings>();
            container.RegisterInstance<ILogger, Logger>();
            container.RegisterInstance<IErrorHandler, ErrorHandler>();
            container.RegisterInstance<IApplication, Application>();
            container.RegisterInstance<IConfigurationManager, ConfigurationManager>();
            container.RegisterInstance<IController, Controller>();

            RegistrationBuilder registrationBuilder = new RegistrationBuilder();
            IApplication application = (IApplication)registrationBuilder.BuildServiceMethodInvoker(container, container.ModuleBuilder, typeof(IApplication))();
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

        private static readonly ErrorHandler s_ErrorHandler = new ErrorHandler(s_Logger, new Settings(new ConfigurationManager()));

        public static IApplication CreateInstance()
        {
            return new Application(new Controller(s_ErrorHandler));
        }
    }
}
