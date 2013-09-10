// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmitHelper.cs" company="Labo">
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
//   Defines the EmitHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Reflection
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Emit helper class.
    /// </summary>
    public static class EmitHelper
    {
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

            DynamicMethod dynamicMethod = new DynamicMethod("ctor", MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, typeof(object), new[] { typeof(object) }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();

            if (type.IsValueType && parameterTypes.Length == 0)
            {
                generator.DeclareLocal(type);
                LdlocaS(generator, 0);
                Initobj(generator, type);
                Ldloc0(generator);
            }
            else
            {
                if (constructorInfo == null)
                {
                    throw new ArgumentNullException("constructorInfo");
                }

                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    Type parameterType = parameterTypes[i];
                    Ldarg(generator, 0);
                    LdcI4(generator, i);
                    LdelemRef(generator);
                    CastIfNotObject(generator, parameterType);
                }

                Newobj(generator, constructorInfo);
            }

            BoxIfValueType(generator, type);
            Ret(generator);

           return (ConstructorInvoker)dynamicMethod.CreateDelegate(typeof(ConstructorInvoker));
        }

        /// <summary>
        /// Creates a new object or a new instance of a value type, pushing an object reference (type O) onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="constructorInfo">The constructor info.</param>
        public static void Newobj(ILGenerator generator, ConstructorInfo constructorInfo)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            if (constructorInfo == null)
            {
                throw new ArgumentNullException("constructorInfo");
            }

            generator.Emit(OpCodes.Newobj, constructorInfo);
        }

        /// <summary>
        /// Boxes the type of if value.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        /// <param name="type">The type.</param>
        public static void BoxIfValueType(ILGenerator generator, Type type)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsValueType)
            {
                Box(generator, type);
            }
        }

        /// <summary>
        /// Returns from the current method, pushing a return value (if present) from the callee's evaluation stack onto the caller's evaluation stack.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static void Ret(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Converts a value type to an object reference (type O).
        /// </summary>
        /// <param name="generator">The il generator.</param>
        /// <param name="type">The type.</param>
        public static void Box(ILGenerator generator, Type type)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Box, type);
        }

        /// <summary>
        /// Initializes each field of the value type at a specified address to a null reference or a 0 of the appropriate primitive type.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        /// <param name="type">The type.</param>
        public static void Initobj(ILGenerator generator, Type type)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            generator.Emit(OpCodes.Initobj, type);
        }

        /// <summary>
        /// Casts type if not equals to object.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ArgumentNullException">type</exception>
        public static void CastIfNotObject(ILGenerator generator, Type type)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type != typeof(object))
            {
                if (type.IsValueType)
                {
                    UnboxAny(generator, type);
                }
                else
                {
                    Castclass(generator, type);
                }
            }
        }

        /// <summary>
        /// Attempts to cast an object passed by reference to the specified class.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        /// <param name="type">The type.</param>
        public static void Castclass(ILGenerator generator, Type type)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            generator.Emit(OpCodes.Castclass, type);
        }

        /// <summary>
        /// Converts the boxed representation of a type specified in the instruction to its unboxed form.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        /// <param name="type">The type.</param>
        public static void UnboxAny(ILGenerator generator, Type type)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            generator.Emit(OpCodes.Unbox_Any, type);
        }

        /// <summary>
        /// Pushes a supplied value of type int32 onto the evaluation stack as an int32.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        /// <param name="num">The num.</param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static void LdcI4(ILGenerator generator, int num)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldc_I4, num);
        }

        /// <summary>
        /// Loads the element containing an object reference at a specified array index onto the top of the evaluation stack as type O (object reference).
        /// </summary>
        /// <param name="generator">The il generator.</param>
        public static void LdelemRef(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldelem_Ref);
        }

        /// <summary>
        /// Loads the argument at index onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index</exception>
        public static void Ldarg(ILGenerator generator, int index)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            switch (index)
            {
                case 0:
                    Ldarg0(generator);
                    break;
                case 1:
                    Ldarg1(generator);
                    break;
                case 2:
                    Ldarg2(generator);
                    break;
                case 3:
                    Ldarg3(generator);
                    break;
                default:
                    if (index <= byte.MaxValue)
                    {
                        LdargS(generator, (byte)index);
                    }
                    else if (index <= short.MaxValue)
                    {
                        Ldarg(generator, (short)index);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }

                    break;
            }
        }

        /// <summary>
        /// Loads the argument at index 0 onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        public static void Ldarg0(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldarg_0);
        }

        /// <summary>
        /// Loads the argument at index 1 onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        public static void Ldarg1(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldarg_1);
        }

        /// <summary>
        /// Loads the argument at index 2 onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        public static void Ldarg2(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldarg_2);
        }

        /// <summary>
        /// Loads the argument at index 3 onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        public static void Ldarg3(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldarg_3);
        }

        /// <summary>
        /// Loads the argument (referenced by a specified short form index) onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The il generator.</param>
        /// <param name="index">The index.</param>
        public static void LdargS(ILGenerator generator, byte index)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldarg_S, index);
        }

        /// <summary>
        /// Loads the address of the local variable at a specific index onto the evaluation stack, short form.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="index">The index.</param>
        public static void LdlocaS(ILGenerator generator, byte index)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldloca_S, index);
        }

        /// <summary>
        /// Loads the local variable at index 0 onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The generator.</param>
        public static void Ldloc0(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldloc_0);
        }
    }
}
