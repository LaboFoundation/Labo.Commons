namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class CastInstanceGenerator : BaseInstanceGenerator
    {
        private readonly IInstanceGenerator m_InstanceGenerator;

        public CastInstanceGenerator(Type type, IInstanceGenerator instanceGenerator)
            : base(type)
        {
            m_InstanceGenerator = instanceGenerator;
        }

        public override void Generate(ILGenerator generator)
        {
            m_InstanceGenerator.Generate(generator);

            EmitHelper.Castclass(generator, Type);
        }
    }
}