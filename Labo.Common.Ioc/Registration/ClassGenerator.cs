﻿namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

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
}