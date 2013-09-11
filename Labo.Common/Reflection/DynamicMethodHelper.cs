// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicMethodHelper.cs" company="Labo">
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
//   Defines the DynamicMethodHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Reflection
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Dynamic method helper class.
    /// </summary>
    public static class DynamicMethodHelper
    {
        /// <summary>
        /// Emits the property setter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>Member setter delegate.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// type
        /// or
        /// propertyInfo
        /// </exception>
        public static MemberSetter EmitPropertySetter(Type type, PropertyInfo propertyInfo)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            MethodInfo setMethodInfo = propertyInfo.GetSetMethod(true);
            DynamicMethod dynamicSetMethod = CreateDynamicMethod("DynamicSet", MethodAttributes.Static | MethodAttributes.Public, typeof(void), new[] { typeof(object), typeof(object) }, type);
            ILGenerator generator = dynamicSetMethod.GetILGenerator();

            EmitHelper.Ldarg(generator, 0);
            EmitHelper.Ldarg(generator, 1);
            EmitHelper.CastIfNotObject(generator, setMethodInfo.GetParameters()[0].ParameterType);
            
            if (setMethodInfo.IsVirtual)
            {
                EmitHelper.CallVirt(generator, setMethodInfo);
            }
            else
            {
                EmitHelper.Call(generator, setMethodInfo);                
            }

            EmitHelper.Ret(generator);

            return (MemberSetter)dynamicSetMethod.CreateDelegate(typeof(MemberSetter));
        }

        /// <summary>
        /// Emits the property getter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>Member getter delegate.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// type
        /// or
        /// propertyInfo
        /// </exception>
        public static MemberGetter EmitPropertyGetter(Type type, PropertyInfo propertyInfo)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
            DynamicMethod dynamicGetMethod = CreateDynamicMethod("DynamicGet", MethodAttributes.Static | MethodAttributes.Public, typeof(object), new[] { typeof(object) }, type);
            ILGenerator generator = dynamicGetMethod.GetILGenerator();

            EmitHelper.Ldarg(generator, 0);

            if (getMethodInfo.IsVirtual)
            {
                EmitHelper.CallVirt(generator, getMethodInfo);
            }
            else
            {
                EmitHelper.Call(generator, getMethodInfo);
            }

            EmitHelper.BoxIfValueType(generator, getMethodInfo.ReturnType);
            EmitHelper.Ret(generator);

            return (MemberGetter)dynamicGetMethod.CreateDelegate(typeof(MemberGetter));
        }

        /// <summary>
        /// Emits the method invoker.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <returns>Method invoker delegate.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// type
        /// or
        /// methodInfo
        /// </exception>
        public static MethodInvoker EmitMethodInvoker(Type type, MethodInfo methodInfo, Type[] parameterTypes = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            if (parameterTypes == null)
            {
                parameterTypes = Type.EmptyTypes;
            }

            Type returnType = methodInfo.ReturnType;
            bool hasReturnType = returnType != typeof(void);

            DynamicMethod dynamicMethod = CreateDynamicMethod("DynamicMethod", MethodAttributes.Static | MethodAttributes.Public, typeof(object), new[] { typeof(object), typeof(object).MakeArrayType() }, type);
            ILGenerator generator = dynamicMethod.GetILGenerator();

            EmitHelper.Ldarg(generator, 0);

            PushParametersToStack(generator, parameterTypes, 1);

            if (methodInfo.IsVirtual)
            {
                EmitHelper.CallVirt(generator, methodInfo);
            }
            else
            {
                EmitHelper.Call(generator, methodInfo);
            }

            if (hasReturnType)
            {
                EmitHelper.BoxIfValueType(generator, returnType);
            }
            else
            {
                EmitHelper.LdNull(generator);
            }

            EmitHelper.Ret(generator);

            return (MethodInvoker)dynamicMethod.CreateDelegate(typeof(MethodInvoker));
        }

        /// <summary>
        /// Emits the constructor invoker.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="constructorInfo">The constructor info.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <returns>constructor invoker.</returns>
        public static ConstructorInvoker EmitConstructorInvoker(Type type, ConstructorInfo constructorInfo = null, Type[] parameterTypes = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (parameterTypes == null)
            {
                parameterTypes = Type.EmptyTypes;
            }

            DynamicMethod dynamicMethod = CreateDynamicMethod("ctor", MethodAttributes.Static | MethodAttributes.Public, typeof(object), new[] { typeof(object) }, type);
            ILGenerator generator = dynamicMethod.GetILGenerator();

            if (type.IsValueType && parameterTypes.Length == 0)
            {
                generator.DeclareLocal(type);
                EmitHelper.LdlocaS(generator, 0);
                EmitHelper.Initobj(generator, type);
                EmitHelper.Ldloc0(generator);
            }
            else
            {
                if (constructorInfo == null)
                {
                    throw new ArgumentNullException("constructorInfo");
                }

                PushParametersToStack(generator, parameterTypes, 0);

                EmitHelper.Newobj(generator, constructorInfo);
            }

            EmitHelper.BoxIfValueType(generator, type);
            EmitHelper.Ret(generator);

            return (ConstructorInvoker)dynamicMethod.CreateDelegate(typeof(ConstructorInvoker));
        }

        /// <summary>
        /// Creates the dynamic method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="methodAttributes">The method attributes.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="ownerType">Type of the owner.</param>
        /// <returns>dynamic method</returns>
        public static DynamicMethod CreateDynamicMethod(string methodName, MethodAttributes methodAttributes, Type returnType, Type[] parameterTypes, Type ownerType)
        {
            return new DynamicMethod(methodName, methodAttributes, CallingConventions.Standard, returnType, parameterTypes, ownerType, true);
        }

        /// <summary>
        /// Pushes the parameters automatic stack.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="arrayParameterIndex">Index of the array parameter.</param>
        private static void PushParametersToStack(ILGenerator generator, Type[] parameterTypes, int arrayParameterIndex)
        {
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                Type parameterType = parameterTypes[i];
                EmitHelper.Ldarg(generator, arrayParameterIndex);
                EmitHelper.LdcI4(generator, i);
                EmitHelper.LdelemRef(generator);
                EmitHelper.CastIfNotObject(generator, parameterType);
            }
        }
    }
}
