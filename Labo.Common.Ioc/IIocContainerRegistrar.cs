﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIocContainerRegistrar.cs" company="Labo">
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
//   Inversion of control container registrar.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Inversion of control container registrar. 
    /// </summary>
    public interface IIocContainerRegistrar
    {
        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator);

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name);

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
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="assemblies">The assemblies.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        void RegisterAssemblyTypes<TService>(params Assembly[] assemblies);

        /// <summary>
        /// Registers the assembly types.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="assemblies">The assemblies.</param>
        void RegisterAssemblyTypes(Type type, params Assembly[] assemblies);
    }
}