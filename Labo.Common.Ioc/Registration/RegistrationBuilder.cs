// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationBuilder.cs" company="Labo">
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
//   Defines the RegistrationBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    using Labo.Common.Ioc.Exceptions;
    using Labo.Common.Ioc.Resources;
    using Labo.Common.Reflection;

    public sealed class RegistrationBuilder
    {
        /// <summary>
        /// The constructor binding flags.
        /// </summary>
        private const BindingFlags CONSTRUCTOR_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// The type counter
        /// </summary>
        private static long s_TypeCounter;

        /// <summary>
        /// The static instance creators.
        /// </summary>
        private readonly List<Func<object>> m_StaticInstanceCreators;

        /// <summary>
        /// The type field counter.
        /// </summary>
        private long m_TypeFieldCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationBuilder"/> class.
        /// </summary>
        public RegistrationBuilder()
        {
            m_StaticInstanceCreators = new List<Func<object>>();
        }

        /// <summary>
        /// Builds the service invoker method.
        /// </summary>
        /// <param name="serviceRegistryProvider">The service registry provider.</param>
        /// <param name="moduleBuilder">The module builder.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceName">The name of the service.</param>
        /// <returns>Service invoker delegate.</returns>
        public Func<object> BuildServiceInvokerMethod(ILaboIocServiceRegistryProvider serviceRegistryProvider, ModuleBuilder moduleBuilder, Type serviceType, string serviceName = null)
        {
            ClassGenerator classGenerator = new ClassGenerator(moduleBuilder, string.Format(CultureInfo.InvariantCulture, "ServiceFactory{0}", GetTypeId()), TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);
            MethodGenerator createInstanceMethodGenerator = new MethodGenerator(classGenerator, "CreateInstance", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig, BuildInstanceGenerator(serviceRegistryProvider, classGenerator, serviceType, serviceName));
            classGenerator.AddMethod(createInstanceMethodGenerator);

            MethodGenerator setSingletonInstancesMethodGenerator = new MethodGenerator(classGenerator, "SetSingletonInstances", MethodAttributes.Public | MethodAttributes.Static, null);
            classGenerator.AddMethod(setSingletonInstancesMethodGenerator);

            Type serviceFactoryType = classGenerator.Generate();

            DynamicMethod createServiceInstanceDynamicMethod = DynamicMethodHelper.CreateDynamicMethod("CreateServiceInstance", MethodAttributes.Static | MethodAttributes.Public, typeof(object), Type.EmptyTypes, serviceFactoryType);
            ILGenerator dynamicMethodGenerator = createServiceInstanceDynamicMethod.GetILGenerator();
            EmitHelper.Call(dynamicMethodGenerator, serviceFactoryType.GetMethod("CreateInstance"));
            EmitHelper.BoxIfValueType(dynamicMethodGenerator, typeof(object));
            EmitHelper.Ret(dynamicMethodGenerator);

            return (Func<object>)createServiceInstanceDynamicMethod.CreateDelegate(typeof(Func<object>));
        }

        /// <summary>
        /// Gets the constructor info of the service implementation type.
        /// </summary>
        /// <param name="serviceImplementationType">Type of the service implementation.</param>
        /// <returns>The constructor info.</returns>
        /// <exception cref="IocContainerDependencyResolutionException">Thrown when no suited constructor is found.</exception>
        private static ConstructorInfo GetConstructorInfo(Type serviceImplementationType)
        {
            ConstructorInfo constructor = serviceImplementationType.GetConstructors(CONSTRUCTOR_BINDING_FLAGS).FirstOrDefault();

            if (constructor == null)
            {
                throw new IocContainerDependencyResolutionException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.LaboIocEmitServiceCreator_CreateServiceInstance_NoConstructorsCanBeFound,
                        serviceImplementationType.FullName));
            }

            return constructor;
        }

        /// <summary>
        /// Gets the type id.
        /// </summary>
        /// <returns>Next type unique id.</returns>
        private static long GetTypeId()
        {
            return Interlocked.Increment(ref s_TypeCounter);
        }

        /// <summary>
        /// Builds the instance generator.
        /// </summary>
        /// <param name="serviceRegistryProvider">The service registry provider.</param>
        /// <param name="classGenerator">The class generator.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceName">The name of the service.</param>
        /// <returns>The instance generator.</returns>
        private IInstanceGenerator BuildInstanceGenerator(ILaboIocServiceRegistryProvider serviceRegistryProvider, ClassGenerator classGenerator, Type serviceType, string serviceName = null)
        {
            LaboIocServiceRegistration serviceRegistryEntry = serviceRegistryProvider.GetServiceRegistryEntry(serviceType, serviceName);
            Type serviceImplementationType = serviceRegistryEntry.ImplementationType;
            ConstructorInfo constructor = GetConstructorInfo(serviceImplementationType);

            ParameterInfo[] constructorParameters = constructor.GetParameters();
            int constructorParametersLength = constructorParameters.Length;
            IInstanceGenerator[] childServices = new IInstanceGenerator[constructorParametersLength];
            for (int i = 0; i < constructorParametersLength; i++)
            {
                ParameterInfo parameterInfo = constructorParameters[i];
                childServices[i] = BuildInstanceGenerator(serviceRegistryProvider, classGenerator, parameterInfo.ParameterType);
            }

            if (serviceRegistryEntry.InstanceCreator != null)
            {
                FieldGenerator fieldGenerator = new FieldGenerator(classGenerator, string.Format(CultureInfo.InvariantCulture, "fld{0}", GetTypeFieldId()), null, FieldAttributes.Private | FieldAttributes.Static);
                classGenerator.AddField(fieldGenerator);
                m_StaticInstanceCreators.Add(serviceRegistryEntry.InstanceCreator);
                return new LoadFieldGenerator(fieldGenerator);
            }
            else
            {
                InstanceGenerator serviceInstanceGenerator = new InstanceGenerator(serviceImplementationType, constructor, childServices);
                if (serviceRegistryEntry.Lifetime == LaboIocServiceLifetime.Singleton)
                {
                    FieldGenerator fieldGenerator = new FieldGenerator(classGenerator, string.Format(CultureInfo.InvariantCulture, "fld{0}", GetTypeFieldId()), serviceInstanceGenerator, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);
                    classGenerator.AddField(fieldGenerator);
                    return new LoadFieldGenerator(fieldGenerator);
                }
                else
                {
                    return serviceInstanceGenerator;
                }
            }
        }

        /// <summary>
        /// Gets the type field id.
        /// </summary>
        /// <returns>Next type field unique id.</returns>
        private long GetTypeFieldId()
        {
            return Interlocked.Increment(ref m_TypeFieldCounter);
        }
    }
}