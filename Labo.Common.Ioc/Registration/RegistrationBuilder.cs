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
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    using Labo.Common.Ioc.Exceptions;
    using Labo.Common.Ioc.Resources;
    using Labo.Common.Reflection;

    public interface IInstanceGenerator
    {
        void Generate(ILGenerator generator);

        Type Type { get; }

        CastInstanceGenerator Cast(Type type);
    }

    public abstract class BaseInstanceGenerator : IInstanceGenerator
    {
        public abstract void Generate(ILGenerator generator);

        private readonly Type m_Type;
        public Type Type
        {
            get
            {
                return m_Type;
            }
        }

        protected BaseInstanceGenerator(Type type)
        {
            m_Type = type;
        }

        public CastInstanceGenerator Cast(Type type)
        {
            return new CastInstanceGenerator(type, this);
        }
    }

    public sealed class LoadFieldGenerator : BaseInstanceGenerator
    {
        private readonly FieldGenerator m_FieldGenerator;

        public LoadFieldGenerator(FieldGenerator fieldGenerator)
            : base(fieldGenerator.Type)
        {
            m_FieldGenerator = fieldGenerator;
        }

        public override void Generate(ILGenerator generator)
        {
            FieldBuilder fieldBuilder = m_FieldGenerator.FieldBuilder;
            if (m_FieldGenerator.IsStatic)
            {
                EmitHelper.Ldsfld(generator, fieldBuilder);
            }
            else
            {
                EmitHelper.Ldfld(generator, fieldBuilder);
            }
        }
    }

    public sealed class FieldGenerator : BaseInstanceGenerator
    {
        private readonly string m_FieldName;

        private readonly IInstanceGenerator m_InstanceGenerator;

        private readonly FieldAttributes m_FieldAttributes;

        public FieldAttributes FieldAttributes
        {
            get
            {
                return m_FieldAttributes;
            }
        }

        public IInstanceGenerator InstanceGenerator
        {
            get
            {
                return m_InstanceGenerator;
            }
        }

        public ClassGenerator Owner { get; internal set; }

        public bool IsStatic { get{ return (FieldAttributes & FieldAttributes.Static) == FieldAttributes.Static; } }

        private readonly Lazy<FieldBuilder> m_FieldBuilder;

        public FieldBuilder FieldBuilder
        {
            get
            {
                return m_FieldBuilder.Value;
            }
        }

        public FieldGenerator(ClassGenerator owner, string fieldName, IInstanceGenerator instanceBuilder, FieldAttributes fieldAttributes = FieldAttributes.Private)
            : base(instanceBuilder.Type)
        {
            Owner = owner;
            m_FieldName = fieldName;
            m_InstanceGenerator = instanceBuilder;
            m_FieldAttributes = fieldAttributes;

            m_FieldBuilder = new Lazy<FieldBuilder>(() => Owner.TypeBuilder.DefineField(m_FieldName, m_InstanceGenerator.Type, m_FieldAttributes));
        }

        public override void Generate(ILGenerator generator)
        {
            m_InstanceGenerator.Generate(generator);

            FieldBuilder fieldBuilder = m_FieldBuilder.Value;
            if (IsStatic)
            {
                EmitHelper.Stsfld(generator, fieldBuilder);
            }
            else
            {
                EmitHelper.Stsfld(generator, fieldBuilder);
            }
        }
    }

    public sealed class CastInstanceGenerator : BaseInstanceGenerator
    {
        private readonly IInstanceGenerator m_InstanceGenerator;

        public CastInstanceGenerator(Type type, IInstanceGenerator instanceGenerator)
            : base(type)
        {
            m_InstanceGenerator = instanceGenerator;
        }

        public override void Generate(ILGenerator generator)
        {
            m_InstanceGenerator.Generate(generator);

            EmitHelper.Castclass(generator, Type);
        }
    }

    public sealed class FuncInstanceGenerator<T> : BaseInstanceGenerator
    {
        private readonly Func<T> m_InstanceCreator;

        private readonly ClassGenerator m_Owner;

        public FuncInstanceGenerator(Func<T> instanceCreator, ClassGenerator owner)
            : base(typeof(T))
        {
            m_InstanceCreator = instanceCreator;
            m_Owner = owner;
        }

        public override void Generate(ILGenerator generator)
        {
            MethodBuilder method = m_Owner.TypeBuilder.DefineMethod("Asd", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig);
            
            Expression expression = Expression.Invoke(Expression.Constant(m_InstanceCreator));
            Func<T> creator = Expression.Lambda<Func<T>>(expression, new ParameterExpression[0]).Compile();
            Expression<Func<T>> creatorExpression = () => creator();

            creatorExpression.CompileToMethod(method);

            EmitHelper.Call(generator, method);
        }
    }

    public sealed class InstanceGenerator : BaseInstanceGenerator
    {
        private readonly ConstructorInfo m_Constructor;

        private readonly List<IInstanceGenerator> m_Parameters;

        public InstanceGenerator(Type type, ConstructorInfo constructor, params IInstanceGenerator[] parameters)
            : base(type)
        {
            m_Constructor = constructor;

            m_Parameters = new List<IInstanceGenerator>(parameters.Length);
            for (int i = 0; i < parameters.Length; i++)
            {
                IInstanceGenerator instanceGenerator = parameters[i];
                m_Parameters.Add(instanceGenerator);
            }
        }

        public override void Generate(ILGenerator generator)
        {
            for (int i = 0; i < m_Parameters.Count; i++)
            {
                IInstanceGenerator instanceGenerator = m_Parameters[i];
                instanceGenerator.Generate(generator);
            }

            EmitHelper.Newobj(generator, m_Constructor);
        }
    }

    public sealed class MethodGenerator
    {
        private readonly string m_Name;

        private readonly MethodAttributes m_MethodAttributes;

        private readonly IInstanceGenerator m_ReturnValue;

        public bool IsStatic { get { return (m_MethodAttributes & MethodAttributes.Static) == MethodAttributes.Static; } }

        public ClassGenerator Owner { get; internal set; }

        public MethodGenerator(ClassGenerator owner, string name, MethodAttributes methodAttributes, IInstanceGenerator returnValue)
        {
            m_Name = name;
            m_MethodAttributes = methodAttributes;
            m_ReturnValue = returnValue;
            Owner = owner;
        }

        public void Generate()
        {
            MethodBuilder methodBuilder = Owner.TypeBuilder.DefineMethod(m_Name, m_MethodAttributes, m_ReturnValue.Type, Type.EmptyTypes);
            ILGenerator methodGenerator = methodBuilder.GetILGenerator();
            m_ReturnValue.Generate(methodGenerator);

            EmitHelper.Ret(methodGenerator);
        }
    }

    public sealed class ClassGenerator
    {
        private readonly TypeAttributes m_TypeAttributes;

        private readonly List<FieldGenerator> m_FieldGenerators;

        private readonly List<MethodGenerator> m_MethodGenerators;

        private readonly TypeBuilder m_TypeBuilder;

        public TypeBuilder TypeBuilder
        {
            get
            {
                return m_TypeBuilder;
            }
        }

        public bool IsStatic { get { return (m_TypeAttributes & (TypeAttributes.Abstract | TypeAttributes.Sealed)) == (TypeAttributes.Abstract | TypeAttributes.Sealed); } }

        public ClassGenerator(ModuleBuilder moduleBuilder, string className, TypeAttributes typeAttributes)
        {
            m_TypeAttributes = typeAttributes;
            m_FieldGenerators = new List<FieldGenerator>();
            m_MethodGenerators = new List<MethodGenerator>();

            m_TypeBuilder = moduleBuilder.DefineType(className, typeAttributes);
        }

        public void AddField(FieldGenerator fieldBuilder)
        {
            fieldBuilder.Owner = this;

            m_FieldGenerators.Add(fieldBuilder);
        }

        public void AddMethod(MethodGenerator methodGenerator)
        {
            methodGenerator.Owner = this;

            m_MethodGenerators.Add(methodGenerator);
        }

        public Type Generate()
        {
            ConstructorBuilder staticConstructorBuilder = m_TypeBuilder.DefineTypeInitializer();
            ILGenerator staticConstructorGenerator = staticConstructorBuilder.GetILGenerator();

            if (!IsStatic)
            {
                ConstructorBuilder defaultConstructorBuilder = m_TypeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
                ILGenerator defaultConstructorGenerator = defaultConstructorBuilder.GetILGenerator();

                for (int i = 0; i < m_FieldGenerators.Count; i++)
                {
                    FieldGenerator fieldGenerator = m_FieldGenerators[i];
                    fieldGenerator.Generate(fieldGenerator.IsStatic ? staticConstructorGenerator : defaultConstructorGenerator);
                }

                EmitHelper.Ret(defaultConstructorGenerator);
            }
            else
            {
                for (int i = 0; i < m_FieldGenerators.Count; i++)
                {
                    FieldGenerator fieldGenerator = m_FieldGenerators[i];
                    fieldGenerator.Generate(staticConstructorGenerator);
                }
            }

            EmitHelper.Ret(staticConstructorGenerator);

            for (int i = 0; i < m_MethodGenerators.Count; i++)
            {
                MethodGenerator methodGenerator = m_MethodGenerators[i];
                methodGenerator.Generate();
            }

            return TypeBuilder.CreateType();
        }
    }

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
        /// The type field counter
        /// </summary>
        private long m_TypeFieldCounter;

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
            MethodGenerator createInstanceMethodGenerator = new MethodGenerator(classGenerator, "CreateInstance", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig, this.BuildInstanceGenerator(serviceRegistryProvider, classGenerator, serviceType, serviceName));
            classGenerator.AddMethod(createInstanceMethodGenerator);
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

            InstanceGenerator serviceInstanceGenerator = new InstanceGenerator(serviceImplementationType, constructor, childServices);
            if (serviceRegistryEntry.Lifetime == LaboIocServiceLifetime.Singleton)
            {
                FieldGenerator fieldGenerator = new FieldGenerator(classGenerator, string.Format(CultureInfo.InvariantCulture, "fld{0}", this.GetTypeFieldId()), serviceInstanceGenerator, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);
                classGenerator.AddField(fieldGenerator);
                return new LoadFieldGenerator(fieldGenerator);
            }
            else
            {
                return serviceInstanceGenerator;
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