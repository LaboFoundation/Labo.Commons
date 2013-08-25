using System;

namespace Labo.Common.Utils
{
    public struct MonthName : IEquatable<MonthName>
    {
        public int Number { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(MonthName other)
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
                return (Number*397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }

        public static bool operator ==(MonthName left, MonthName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MonthName left, MonthName right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MonthName && Equals((MonthName) obj);
        }
    }
}