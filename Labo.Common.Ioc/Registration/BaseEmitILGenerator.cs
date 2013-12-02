namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Reflection.Emit;

    public abstract class BaseEmitILGenerator
    {
        private readonly Type m_Type;

        public Type Type
        {
            get
            {
                return m_Type;
            }
        }

        public abstract void Generate(ILGenerator generator);

        public BaseEmitILGenerator(Type type)
        {
            m_Type = type;
        }
    }
}