// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LaboIocServiceCreationInfo.cs" company="Labo">
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
//   Defines the LaboIocServiceCreationInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;
    using System.Reflection;

    using Labo.Common.Utils;

    internal sealed class LaboIocServiceCreationInfo
    {
        private readonly ConstructorInfo m_ServiceConstructor;

        private readonly Type[] m_DependentServiceTypes;

        private readonly ILaboIocLifetimeManagerProvider m_LifetimeManagerProvider;

        //private readonly Func<object>[] m_DependentServiceCreators;

        public Type[] DependentServiceTypes
        {
            get
            {
                return m_DependentServiceTypes;
            }
        }

        public Func<object>[] DependentServiceCreators;

        public ConstructorInfo ServiceConstructor
        {
            get
            {
                return m_ServiceConstructor;
            }
        }

        public LaboIocServiceCreationInfo(ConstructorInfo serviceConstructor, ILaboIocLifetimeManagerProvider lifetimeManagerProvider)
        {
            m_ServiceConstructor = serviceConstructor;
            m_LifetimeManagerProvider = lifetimeManagerProvider;
            
            ParameterInfo[] contructorParameterInfos = m_ServiceConstructor.GetParameters();
            int constructorParametersLength = contructorParameterInfos.Length;
            m_DependentServiceTypes = new Type[constructorParametersLength];

            DependentServiceCreators = new Func<object>[constructorParametersLength];
            for (int i = 0; i < constructorParametersLength; i++)
            {
                ParameterInfo contructorParameterInfo = contructorParameterInfos[i];
                Type dependentServiceType = contructorParameterInfo.ParameterType;
                m_DependentServiceTypes[i] = dependentServiceType;

                ILaboIocServiceLifetimeManager serviceLifetimeManager = m_LifetimeManagerProvider.GetServiceLifetimeManager(dependentServiceType);
                if (serviceLifetimeManager == null)
                {
                    DependentServiceCreators[i] = () => TypeUtils.GetDefaultValueOfType(dependentServiceType);
                }
                else
                {
                    DependentServiceCreators[i] = serviceLifetimeManager.GetServiceInstanceCreator();
                }
            }
        }
    }
}
