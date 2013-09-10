// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstructorInvoker.cs" company="Labo">
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
//   Defines the ConstructorInvoker type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Reflection
{
    /// <summary>
    /// A delegate to construct an instance of an object.
    /// </summary>
    /// <param name="parameters">The constructor parameters.</param>
    /// <returns>Object instance.</returns>
    public delegate object ConstructorInvoker(params object[] parameters);

    /// <summary>
    /// A delegate to call an objects method.
    /// </summary>
    /// <param name="obj">The object instance that is going to used for method invocation.</param>
    /// <param name="parameters">The method parameters.</param>
    /// <returns>Method return value.</returns>
    public delegate object MethodInvoker(object obj, params object[] parameters);

    /// <summary>
    /// A delegate to get value of a field or property.
    /// </summary>
    /// <param name="obj">
    /// The object instance to get field or property value.
    /// </param>
    /// <returns>The field or property value.</returns>
    public delegate object MemberGetter(object obj);

    /// <summary>
    /// A delegate to set value of a field or property.
    /// </summary>
    /// <param name="obj">
    /// The object instance to get field or property value.
    /// </param>
    /// <param name="value">The value to be set to the field or property.</param>
    public delegate void MemberSetter(object obj, object value);
}
