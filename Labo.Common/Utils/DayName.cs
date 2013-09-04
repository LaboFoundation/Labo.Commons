// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DayName.cs" company="Labo">
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
//   Defines the DayName type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Utils
{
    using System;

    /// <summary>
    /// Day name struct.
    /// </summary>
    public struct DayName : IEquatable<DayName>
    {
        /// <summary>
        /// Gets or sets the day number.
        /// </summary>
        /// <value>
        /// The day number.
        /// </value>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///  Determines whether two specified instances of <see cref="T:Labo.Common.Utils.DayName"/> are equal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns> true if <paramref name="left"/> and <paramref name="right"/> represent the same DayName; otherwise, false.</returns>
        public static bool operator ==(DayName left, DayName right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two specified instances of <see cref="T:Labo.Common.Utils.DayName"/> are not equal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns> true if <paramref name="left"/> and <paramref name="right"/> do not represent the same DayName; otherwise, false.</returns>
        public static bool operator !=(DayName left, DayName right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(DayName other)
        {
            return Number == other.Number && string.Equals(Name, other.Name);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Number * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DayName && Equals((DayName)obj);
        }
    }
}