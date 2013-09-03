using System;
using System.Collections.Generic;
using System.Reflection;

namespace Labo.Common.Ioc
{
    public interface IIocContainer
    {
        void RegisterSingleInstance<T>(Func<IIocContainer, T> @delegate);

        void RegisterSingleInstanceNamed<T>(Func<IIocContainer, T> @delegate, string name);

        void RegisterSingleInstance<TService, TImplementation>();

        void RegisterInstance<TService, TImplementation>();

        void RegisterSingleInstance<TService, TImplementation>(Func<IIocContainer, TImplementation> @delegate)
            where TImplementation : TService;

        bool IsRegistered<T>(string key);

        bool IsRegistered<T>();

        void RegisterAssemblyTypes<T>(params Assembly[] assemblies);

        void RegisterAssemblyTypes(Type type, params Assembly[] assemblies);

        IEnumerable<TService> GetAllInstances<TService>();

        IEnumerable<object> GetAllInstances(Type serviceType);

        TService GetInstance<TService>(params object[] parameters);

        TService GetInstance<TService>(string key);

        object GetInstance(Type serviceType, params object[] parameters);

        object GetInstance(Type serviceType, string key);

        object GetInstanceOptional(Type serviceType, params object[] parameters);

        bool IsRegistered(Type type);

        void RegisterInstance(Type serviceType);

        void RegisterSingleInstance(Type serviceType);
    }
}
