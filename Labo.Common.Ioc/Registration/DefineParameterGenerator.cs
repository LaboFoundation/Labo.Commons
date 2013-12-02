namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public sealed class DefineParameterGenerator : BaseEmitILGenerator
    {
        private readonly string m_Name;

        public MethodBuilder MethodBuilder { get; internal set; }

        public MethodGenerator MethodGenerator { get; internal set; }

        public int Position { get; internal set; }

        public DefineParameterGenerator(Type type, string name = null)
            : base(type)
        {
            m_Name = name;
        }

        public override void Generate(ILGenerator generator)
        {
            MethodBuilder.DefineParameter(Position, ParameterAttributes.None, m_Name);
        }
    }
}