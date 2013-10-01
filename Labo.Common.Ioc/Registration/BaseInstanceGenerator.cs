namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Reflection.Emit;

    public abstract class BaseInstanceGenerator : IInstanceGenerator
    {
        public abstract void Generate(ILGenerator generator);

        private readonly Type m_Type;
        public Type Type
        {
            get
            {
                return m_Type;
            }
        }

        protected BaseInstanceGenerator(Type type)
        {
            m_Type = type;
        }

        public CastInstanceGenerator Cast(Type type)
        {
            return new CastInstanceGenerator(type, this);
        }
    }
}