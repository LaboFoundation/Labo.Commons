namespace Labo.Common.Ioc.Registration
{
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class StoreFieldGenerator : BaseInstanceGenerator
    {
        private readonly FieldGenerator m_FieldGenerator;

        private readonly IInstanceGenerator m_InstanceGenerator;

        public StoreFieldGenerator(FieldGenerator fieldGenerator, IInstanceGenerator instanceGenerator)
            : base(fieldGenerator.Type)
        {
            m_FieldGenerator = fieldGenerator;
            m_InstanceGenerator = instanceGenerator;
        }

        public override void Generate(ILGenerator generator)
        {
            m_InstanceGenerator.Generate(generator);

            FieldBuilder fieldBuilder = m_FieldGenerator.FieldBuilder;
            if (m_FieldGenerator.IsStatic)
            {
                EmitHelper.Stsfld(generator, fieldBuilder);
            }
            else
            {
                EmitHelper.Stfld(generator, fieldBuilder);
            }
        }
    }
}