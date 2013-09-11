// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LaboIocEmitServiceCreator.cs" company="Labo">
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
//   Reflection emit service creator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;

    using Labo.Common.Reflection;
    using Labo.Common.Utils;

    /// <summary>
    /// Reflection emit service creator.
    /// </summary>
    internal sealed class LaboIocEmitServiceCreator : ILaboIocServiceCreator
    {
        /// <summary>
        /// The constructor invoker delegate.
        /// </summary>
        private readonly ConcurrentDictionary<ConstructorInfo, ConstructorInvoker> m_ConstructorInvokerCache;

        /// <summary>
        /// The service implementation type
        /// </summary>
        private readonly Type m_ServiceImplementationType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaboIocEmitServiceCreator"/> class.
        /// </summary>
        /// <param name="serviceImplemetationType">Type of the service implementation.</param>
        public LaboIocEmitServiceCreator(Type serviceImplemetationType)
        {
            m_ServiceImplementationType = serviceImplemetationType;

            m_ConstructorInvokerCache = new ConcurrentDictionary<ConstructorInfo, ConstructorInvoker>();
        }

        /// <summary>
        /// Creates the service instance.
        /// </summary>
        /// <param name="containerResolver">Container resolver.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Service instance.</returns>
        public object CreateServiceInstance(IIocContainerResolver containerResolver, params object[] parameters)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            ConstructorInfo constructor;
            Type[] parameterTypes;

            if (parameters.Length > 0)
            {
                parameterTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    object parameter = parameters[i];
                    parameterTypes[i] = TypeUtils.GetType(parameter);
                }

                constructor = m_ServiceImplementationType.GetConstructor(bindingFlags, null, parameterTypes, null);
            }
            else
            {
                constructor = m_ServiceImplementationType.GetConstructors(bindingFlags).FirstOrDefault();

                if (constructor == null)
                {
                    throw new InvalidOperationException();
                }

                ParameterInfo[] constructorParameters = constructor.GetParameters();
                int constructorParametersLength = constructorParameters.Length;
                parameters = new object[constructorParametersLength];
                parameterTypes = new Type[constructorParametersLength];
                for (int i = 0; i < constructorParametersLength; i++)
                {
                    ParameterInfo constructorParameter = constructorParameters[i];
                    parameterTypes[i] = constructorParameter.ParameterType;
                    parameters[i] = containerResolver.GetInstance(constructorParameter.ParameterType);
                }
            }

            return m_ConstructorInvokerCache.GetOrAdd(constructor, c => DynamicMethodHelper.EmitConstructorInvoker(m_ServiceImplementationType, c, parameterTypes))(parameters);
        }
    }
}