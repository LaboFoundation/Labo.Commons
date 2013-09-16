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
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    using Labo.Common.Reflection;

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