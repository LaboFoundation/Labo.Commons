// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FFastInjectorIocContainer.cs" company="Labo">
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
//   FFastInjector inversion of control container class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.FFastInjector
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using global::fFastInjector;

    /// <summary>
    /// FFastInjector inversion of control container class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public sealed class FFastInjectorIocContainer : BaseIocContainer
    {
        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        public override void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            Injector.SetResolver(() => creator(this));
        }

        public override void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            throw new NotImplementedException();
        }

        public override void RegisterSingleInstance(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public override void RegisterInstance(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public override void RegisterSingleInstance(Type serviceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        public override void RegisterSingleInstanceNamed(Type serviceType, Type implementationType, string name)
        {
            throw new NotImplementedException();
        }

        public override void RegisterSingleInstanceNamed(Type serviceType, string name)
        {
            throw new NotImplementedException();
        }

        public override void RegisterInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            throw new NotImplementedException();
        }

        public override void RegisterInstance(Type serviceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        public override void RegisterInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            throw new NotImplementedException();
        }

        public override void RegisterInstanceNamed(Type serviceType, Type implementationType, string name)
        {
            throw new NotImplementedException();
        }

        public override void RegisterInstanceNamed(Type serviceType, string name)
        {
            throw new NotImplementedException();
        }

        public override object GetInstance(Type serviceType, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override object GetInstanceByName(Type serviceType, string name, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override object GetInstanceOptional(Type serviceType, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override object GetInstanceOptionalByName(Type serviceType, string name, params object[] parameters)
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
