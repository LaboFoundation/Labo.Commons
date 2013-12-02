namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class CastClassOperationGenerator : BaseEmitILGenerator
    {
        private readonly IInstanceGenerator m_InstanceGenerator;

        public CastClassOperationGenerator(Type type, IInstanceGenerator instanceGenerator)
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