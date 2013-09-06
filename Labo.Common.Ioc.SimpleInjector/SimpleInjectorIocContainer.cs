using System;
using System.Collections.Generic;

namespace Labo.Common.Ioc.SimpleInjector
{
    public sealed class SimpleInjectorIocContainer : BaseIocContainer
    {
        public override void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            throw new NotImplementedException();
        }

        public override void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            throw new NotImplementedException();
        }

        public override void RegisterSingleInstance<TService, TImplementation>()
        {
            throw new NotImplementedException();
        }

        public override void RegisterSingleInstanceNamed<TService, TImplementation>(string name)
        {
            throw new NotImplementedException();
        }

        public override void RegisterSingleInstance(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public override void RegisterInstance<TService, TImplementation>()
        {
            throw new NotImplementedException();
        }

        public override void RegisterInstance(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public override void RegisterAssemblyTypes(Type type, params System.Reflection.Assembly[] assemblies)
        {
            throw new NotImplementedException();
        }

        public override object GetInstance(Type serviceType, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override object GetInstance(Type serviceType, string name)
        {
            throw new NotImplementedException();
        }

        public override object GetInstanceOptional(Type serviceType, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public override bool IsRegistered(Type type)
        {
            throw new NotImplementedException();
        }

        public override bool IsRegistered(Type type, string name)
        {
            throw new NotImplementedException();
        }
    }
}
