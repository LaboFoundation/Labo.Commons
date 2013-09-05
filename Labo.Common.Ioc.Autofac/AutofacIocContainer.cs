// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutofacIocContainer.cs" company="Labo">
//   The MIT License (MIT)
//   
//   Copyright (c) 2013 Bora Akgun
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to
//   use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//   the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the AutofacIocContainer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Autofac
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using global::Autofac;
    using global::Autofac.Core;
    using global::Autofac.Features.Variance;

    /// <summary>
    /// Autofac inversion of control container class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public sealed class AutofacIocContainer : BaseIocContainer
    {
        /// <summary>
        /// The container
        /// </summary>
        private readonly IContainer m_Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacIocContainer"/> class.
        /// </summary>
        public AutofacIocContainer()
        {
            m_Container = new ContainerBuilder().Build();
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        public override void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.Register(x => creator(this))
                .As<TImplementation>()
                .SingleInstance();
            containerBuilder.Update(m_Container);
        }

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public override void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.Register(x => creator(this))
                .Named<TImplementation>(name)
                .As<TImplementation>()
                .SingleInstance();
            containerBuilder.Update(m_Container);
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public override void RegisterSingleInstance<TService, TImplementation>()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TImplementation>()
                .As<TService>()
                .SingleInstance();
            containerBuilder.Update(m_Container);
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public override void RegisterSingleInstance(Type serviceType)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType(serviceType)
                .SingleInstance();
            containerBuilder.Update(m_Container);
        }

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="name">The instance name.</param>
        public override void RegisterSingleInstanceNamed<TService, TImplementation>(string name)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TImplementation>()
                .Named<TImplementation>(name)
                .As<TService>();
            containerBuilder.Update(m_Container);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public override void RegisterInstance<TService, TImplementation>()
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TImplementation>()
                .As<TService>();
            containerBuilder.Update(m_Container);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public override void RegisterInstance(Type serviceType)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType(serviceType)
                .SingleInstance();
            containerBuilder.Update(m_Container);
        }

        /// <summary>
        /// Registers the assembly types.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="assemblies">The assemblies.</param>
        public override void RegisterAssemblyTypes(Type type, params Assembly[] assemblies)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterSource(new ContravariantRegistrationSource());
            containerBuilder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(type);
            containerBuilder.Update(m_Container);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstance(Type serviceType, params object[] parameters)
        {
            return m_Container.Resolve(serviceType, GetAutofacParameters(parameters));
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        public override object GetInstance(Type serviceType, string name)
        {
            return m_Container.ResolveNamed(name, serviceType);
        }

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptional(Type serviceType, params object[] parameters)
        {
            return m_Container.ResolveOptional(serviceType, GetAutofacParameters(parameters));
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>all instances.</returns>
        public override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            Type serviceTypeEnumerable = typeof(IEnumerable<>).MakeGenericType(new[] { serviceType });

            object obj = m_Container.Resolve(serviceTypeEnumerable);
            return ((IEnumerable)obj).Cast<object>();
        }

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRegistered(Type type)
        {
            return m_Container.IsRegistered(type);
        }

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRegistered(Type type, string name)
        {
            return m_Container.IsRegisteredWithKey(name, type);
        }

        /// <summary>
        /// Gets the autofac parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>parameters.</returns>
        private static Parameter[] GetAutofacParameters(params object[] parameters)
        {
            Parameter[] autofacParameters = new Parameter[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                object parameter = parameters[i];
                autofacParameters[i] = new PositionalParameter(i, parameter);
            }

            return autofacParameters;
        }
    }
}
