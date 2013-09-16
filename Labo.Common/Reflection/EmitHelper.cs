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
        /// Pushes a null reference (type O) onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The generator.</param>
        public static void LdNull(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldnull);
        }

        /// <summary>
        ///  Pops the current value from the top of the evaluation stack and stores it in a the local variable list at a specified index.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="index">The index.</param>
        public static void Stloc(ILGenerator generator, short index)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            if (index >= byte.MinValue && index <= byte.MaxValue)
            {
                StlocS(generator, (byte)index);
            }

            generator.Emit(OpCodes.Stloc, index);
        }

        /// <summary>
        ///  Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index (short form).
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="index">The index.</param>
        public static void StlocS(ILGenerator generator, byte index)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            switch (index)
            {
                case 0:
                    Stloc0(generator);
                    break;
                case 1:
                    Stloc1(generator);
                    break;
                case 2:
                    Stloc2(generator);
                    break;
                case 3:
                    Stloc3(generator);
                    break;

                default:
                    generator.Emit(OpCodes.Stloc_S, index);
                    break;
            }
        }

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 0.
        /// </summary>
        /// <param name="generator">The generator.</param>
        public static void Stloc0(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Stloc_0);
        }

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 1.
        /// </summary>
        /// <param name="generator">The generator.</param>
        public static void Stloc1(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Stloc_1);
        }

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 2.
        /// </summary>
        /// <param name="generator">The generator.</param>
        public static void Stloc2(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Stloc_2);
        }

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 3.
        /// </summary>
        /// <param name="generator">The generator.</param>
        public static void Stloc3(ILGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Stloc_3);
        }

        /// <summary>
        /// Replaces the value of a static field with a value from the evaluation stack.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="field">The field.</param>
        /// <exception cref="System.ArgumentNullException">generator</exception>
        public static void Stsfld(ILGenerator generator, FieldInfo field)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Stsfld, field);
        }

        /// <summary>
        /// Pushes the value of a static field onto the evaluation stack..
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="field">The field.</param>
        /// <exception cref="System.ArgumentNullException">generator</exception>
        public static void Ldsfld(ILGenerator generator, FieldInfo field)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldsfld, field);
        }

        /// <summary>
        /// Calls the method indicated by the passed method descriptor.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <exception cref="System.ArgumentNullException">
        /// generator
        /// or
        /// methodInfo
        /// </exception>
        public static void Call(ILGenerator generator, MethodInfo methodInfo)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            generator.Emit(OpCodes.Call, methodInfo);
        }

        /// <summary>
        ///  Calls a late-bound method on an object, pushing the return value onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <exception cref="System.ArgumentNullException">
        /// generator
        /// or
        /// methodInfo
        /// </exception>
        public static void CallVirt(ILGenerator generator, MethodInfo methodInfo)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            generator.Emit(OpCodes.Callvirt, methodInfo);
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
        /// Boxes the type of if it is value type.
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
        /// UnBoxes the type of if it is value type.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ArgumentNullException">
        /// generator
        /// or
        /// type
        /// </exception>
        public static void UnBoxIfValueType(ILGenerator generator, Type type)
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
                UnboxAny(generator, type);
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

        /// <summary>
        /// Loads the local variable at a specific index onto the evaluation stack.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="index">The index.</param>
        /// <exception cref="System.ArgumentNullException">generator</exception>
        public static void Ldloc(ILGenerator generator, short index)
        {
            if (generator == null)
            {
                throw new ArgumentNullException("generator");
            }

            generator.Emit(OpCodes.Ldloc, index);
        }
    }
}
