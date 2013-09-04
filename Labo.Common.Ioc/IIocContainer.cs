using System;
using System.Collections.Generic;
using System.Reflection;

namespace Labo.Common.Ioc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIocContainer
    {
        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        void RegisterSingleInstance<TImplementation>(Func<IIocContainer, TImplementation> creator);

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainer, TImplementation> creator, string name);

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        void RegisterSingleInstance<TService, TImplementation>() 
            where TImplementation : TService;

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="name">The instance name.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        void RegisterSingleInstanceNamed<TService, TImplementation>(string name)
          where TImplementation : TService;

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        void RegisterInstance<TService, TImplementation>() 
            where TImplementation : TService;

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        void RegisterInstance(Type serviceType);

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        void RegisterSingleInstance(Type serviceType);

        /// <summary>
        /// Registers the assembly types.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="assemblies">The assemblies.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        void RegisterAssemblyTypes<TService>(params Assembly[] assemblies);

        /// <summary>
        /// Registers the assembly types.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="assemblies">The assemblies.</param>
        void RegisterAssemblyTypes(Type type, params Assembly[] assemblies);

        /// <summary>
        /// Determines whether the specified key is registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is registered; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        bool IsRegistered<TService>(string key);

        /// <summary>
        /// Determines whether this instance is registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        ///   <c>true</c> if this instance is registered; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        bool IsRegistered<TService>();

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsRegistered(Type type);

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        IEnumerable<TService> GetAllInstances<TService>();

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns></returns>
        IEnumerable<object> GetAllInstances(Type serviceType);

        /// <summary>
        /// Gets the instance with parameters.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        TService GetInstance<TService>(params object[] parameters);

        /// <summary>
        /// Gets the instance by instance name.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The instance name.</param>
        /// <returns></returns>
        TService GetInstance<TService>(string name);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        object GetInstance(Type serviceType, params object[] parameters);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        object GetInstance(Type serviceType, string name);

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        object GetInstanceOptional(Type serviceType, params object[] parameters);
    }
}
