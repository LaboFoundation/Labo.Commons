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

    using Labo.Common.Reflection;

    public interface IInstanceGenerator
    {
        void Generate(ILGenerator generator);

        void Load(ILGenerator generator);

        Type Type { get; }
    }

    public sealed class FieldGenerator : IInstanceGenerator
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

        public Type Type
        {
            get { return InstanceGenerator.Type; }
        }

        public ClassGenerator Owner { get; internal set; }

        public bool IsStatic { get{ return (FieldAttributes & FieldAttributes.Static) == FieldAttributes.Static; } }

        private Lazy<FieldBuilder> m_FieldBuilder; 

        public FieldGenerator(ClassGenerator owner, string fieldName, IInstanceGenerator instanceBuilder, FieldAttributes fieldAttributes = FieldAttributes.Private)
        {
            Owner = owner;
            m_FieldName = fieldName;
            m_InstanceGenerator = instanceBuilder;
            m_FieldAttributes = fieldAttributes;

            m_FieldBuilder = new Lazy<FieldBuilder>(() => Owner.TypeBuilder.DefineField(m_FieldName, m_InstanceGenerator.Type, m_FieldAttributes));
        }

        public void Generate(ILGenerator generator)
        {
            m_InstanceGenerator.Generate(generator);
        }

        public void Load(ILGenerator generator)
        {
            FieldBuilder fieldBuilder = m_FieldBuilder.Value;

            if (IsStatic)
            {
                EmitHelper.Stsfld(generator, fieldBuilder);
                EmitHelper.Ldsfld(generator, fieldBuilder);
            }
            else
            {
                EmitHelper.Stfld(generator, fieldBuilder);
                EmitHelper.Ldfld(generator, fieldBuilder);
            }
        }
    }

    public sealed class CastInstanceGenerator : IInstanceGenerator
    {
        private readonly IInstanceGenerator m_InstanceGenerator;

        public Type Type { get; private set; }

        public CastInstanceGenerator(Type type, IInstanceGenerator instanceGenerator)
        {
            m_InstanceGenerator = instanceGenerator;
            Type = type;
        }

        public void Generate(ILGenerator generator)
        {
            m_InstanceGenerator.Generate(generator);

            EmitHelper.Castclass(generator, Type);
        }

        public void Load(ILGenerator generator)
        {
        }
    }


    public sealed class InstanceGenerator : IInstanceGenerator
    {
        private readonly ConstructorInfo m_Constructor;

        private readonly List<IInstanceGenerator> m_Parameters;

        public Type Type { get; private set; }

        public ConstructorInfo Constructor
        {
            get
            {
                return m_Constructor;
            }
        }

        public InstanceGenerator(Type type, ConstructorInfo constructor, params IInstanceGenerator[] parameters)
        {
            m_Constructor = constructor;
            Type = type;

            m_Parameters = new List<IInstanceGenerator>(parameters.Length);
            for (int i = 0; i < parameters.Length; i++)
            {
                IInstanceGenerator instanceGenerator = parameters[i];
                m_Parameters.Add(instanceGenerator);
            }
        }

        public void Generate(ILGenerator generator)
        {
            for (int i = 0; i < m_Parameters.Count; i++)
            {
                IInstanceGenerator instanceGenerator = m_Parameters[i];
                instanceGenerator.Generate(generator);
                instanceGenerator.Load(generator);
            }

            EmitHelper.Newobj(generator, m_Constructor);
        }

        public void Load(ILGenerator generator)
        {
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

        public void Generate()
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
        }

        public Delegate GetMethod<TDelegate>(string methodName)
        {
            Type type = m_TypeBuilder.CreateType();
            MethodInfo method = type.GetMethod(methodName);

            return Delegate.CreateDelegate(typeof(TDelegate), method);
        }
    }

    public sealed class RegistrationBuilder
    {
        private long m_TypeFieldCounter;

        private static long s_TypeCounter;

        public Func<object> BuildRegistration(ILaboIocLifetimeManagerProvider lifetimeManagerProvider, ModuleBuilder moduleBuilder, Type serviceImplementationType, LaboIocServiceLifetime lifetime)
        {
            ConstructorInfo constructor = serviceImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();
            TypeBuilder typeBuilder = moduleBuilder.DefineType(string.Format(CultureInfo.InvariantCulture, "ServiceFactory{0}", GetTypeId()), TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);
            
            ConstructorBuilder constructorBuilder = typeBuilder.DefineTypeInitializer();
            ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();

            MethodBuilder createInstanceMethodBuilder = typeBuilder.DefineMethod("CreateInstance", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig, typeof(object), Type.EmptyTypes);
            ILGenerator createInstanceMethodGenerator = createInstanceMethodBuilder.GetILGenerator();

            ParameterInfo[] constructorParameters = constructor.GetParameters();
            byte localVariableIndex = 0;
            for (int i = 0; i < constructorParameters.Length; i++)
            {
                ParameterInfo parameterInfo = constructorParameters[i];
                Type serviceType = parameterInfo.ParameterType;
                ILaboIocServiceLifetimeManager serviceLifetimeManager = lifetimeManagerProvider.GetServiceLifetimeManager(serviceType);
                if (serviceLifetimeManager != null)
                {
                    BuildRegistration(lifetimeManagerProvider, typeBuilder, constructorGenerator, createInstanceMethodGenerator, serviceLifetimeManager.ServiceCreator.ServiceImplementationType, serviceLifetimeManager.Lifetime);
                }
                else
                {
                    if (lifetime == LaboIocServiceLifetime.Singleton) // Singleton
                    {
                        FieldBuilder fieldBuilder = typeBuilder.DefineField(string.Format(CultureInfo.InvariantCulture, "field{0}", GetTypeFieldId()), serviceType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);

                        if (serviceType.IsValueType)
                        {
                            EmitHelper.Ldsflda(constructorGenerator, fieldBuilder);
                            EmitHelper.Initobj(constructorGenerator, serviceType);
                        }
                        else
                        {
                            EmitHelper.Stsfld(constructorGenerator, fieldBuilder);
                        }

                        EmitHelper.Ldsfld(constructorGenerator, fieldBuilder);
                    }
                    else
                    {
                        if (serviceType.IsValueType)
                        {
                            EmitHelper.LdlocaS(constructorGenerator, localVariableIndex);
                            EmitHelper.Initobj(constructorGenerator, serviceType);
                            EmitHelper.Ldloc(constructorGenerator, localVariableIndex);

                            localVariableIndex++;
                        }
                        else
                        {
                            EmitHelper.LdNull(constructorGenerator);
                        }
                    }
                }
            }

            if (lifetime == LaboIocServiceLifetime.Singleton) // Singleton
            {
                FieldBuilder fieldBuilder = typeBuilder.DefineField(string.Format(CultureInfo.InvariantCulture, "field{0}", GetTypeFieldId()), serviceImplementationType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);
                EmitHelper.Newobj(constructorGenerator, constructor);
                EmitHelper.Stsfld(constructorGenerator, fieldBuilder);
                EmitHelper.Ret(constructorGenerator);

                EmitHelper.Ldsfld(createInstanceMethodGenerator, fieldBuilder);
                EmitHelper.Ret(createInstanceMethodGenerator);
            }
            else
            {
                EmitHelper.Newobj(createInstanceMethodGenerator, constructor);
                EmitHelper.Ret(createInstanceMethodGenerator);
            }

            Type type = typeBuilder.CreateType();
            MethodInfo createInstanceMethod = type.GetMethod("CreateInstance");

            return (Func<object>)Delegate.CreateDelegate(typeof(Func<object>), createInstanceMethod);
        }

        private void BuildRegistration(ILaboIocLifetimeManagerProvider lifetimeManagerProvider, TypeBuilder typeBuilder, ILGenerator constructorGenerator, ILGenerator createInstanceMethodGenerator, Type serviceImplementationType, LaboIocServiceLifetime currentServiceLifetime)
        {
            ConstructorInfo constructor = serviceImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();

            ParameterInfo[] constructorParameters = constructor.GetParameters();
            for (int i = 0; i < constructorParameters.Length; i++)
            {
                ParameterInfo parameterInfo = constructorParameters[i];
                Type serviceType = parameterInfo.ParameterType;
                ILaboIocServiceLifetimeManager serviceLifetimeManager = lifetimeManagerProvider.GetServiceLifetimeManager(serviceType);
                if (serviceLifetimeManager != null)
                {
                    BuildRegistration(lifetimeManagerProvider, typeBuilder, constructorGenerator, createInstanceMethodGenerator, serviceLifetimeManager.ServiceCreator.ServiceImplementationType, serviceLifetimeManager.Lifetime);
                }
                else
                {
                    FieldBuilder fieldBuilder = typeBuilder.DefineField(string.Format(CultureInfo.InvariantCulture, "field{0}", GetTypeFieldId()), serviceType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);

                    if (serviceType.IsValueType)
                    {
                        EmitHelper.Ldsflda(constructorGenerator, fieldBuilder);
                        EmitHelper.Initobj(constructorGenerator, serviceType);
                    }
                    else
                    {
                        EmitHelper.Stsfld(constructorGenerator, fieldBuilder);
                    }

                    EmitHelper.Ldsfld(constructorGenerator, fieldBuilder);
                }
            }

            if (currentServiceLifetime == LaboIocServiceLifetime.Singleton) // Singleton
            {
                FieldBuilder fieldBuilder = typeBuilder.DefineField(string.Format(CultureInfo.InvariantCulture, "field{0}", GetTypeFieldId()), serviceImplementationType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);
                EmitHelper.Newobj(constructorGenerator, constructor);
                EmitHelper.Stsfld(constructorGenerator, fieldBuilder);

                EmitHelper.Ldsfld(constructorGenerator, fieldBuilder);
            }
            else
            {
                EmitHelper.Newobj(createInstanceMethodGenerator, constructor);
            }
        }

        public long GetTypeFieldId()
        {
            return Interlocked.Increment(ref m_TypeFieldCounter);
        }

        public long GetTypeId()
        {
            return Interlocked.Increment(ref s_TypeCounter);
        }
    }
}