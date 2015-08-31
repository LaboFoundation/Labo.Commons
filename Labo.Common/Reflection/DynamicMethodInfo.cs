namespace Labo.Common.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal struct DynamicMethodInfo : IEquatable<DynamicMethodInfo>
    {
        public Type TargetType { get; private set; }
        public MemberTypes MemberTypes { get; private set; }
        public Type[] ParameterTypes { get; private set; }
        public string Name { get; private set; }

        public DynamicMethodInfo(Type targetType, MemberTypes memberTypes, string name, Type[] parameterTypes)
            : this()
        {
            TargetType = targetType;
            MemberTypes = memberTypes;
            Name = name;
            ParameterTypes = parameterTypes == null || parameterTypes.Length == 0
                             ? Type.EmptyTypes
                             : parameterTypes;
        }

        public bool Equals(DynamicMethodInfo other)
        {
            return TargetType == other.TargetType && MemberTypes == other.MemberTypes && Equals(ParameterTypes, other.ParameterTypes) && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is DynamicMethodInfo && Equals((DynamicMethodInfo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = TargetType != null ? TargetType.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (int)MemberTypes;
                hashCode = (hashCode * 397) ^ (ParameterTypes != null ? ListHashCode(ParameterTypes) : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }

        private static int ListHashCode<T>(IList<T> list)
        {
            EqualityComparer<T> cmp = EqualityComparer<T>.Default;
            int h = 6551;
            for (int i = 0; i < list.Count; i++)
            {
                T t = list[i];
                h ^= (h << 5) ^ cmp.GetHashCode(t);
            }

            return h;
        }

        public static bool operator ==(DynamicMethodInfo left, DynamicMethodInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DynamicMethodInfo left, DynamicMethodInfo right)
        {
            return !left.Equals(right);
        }
    }
}