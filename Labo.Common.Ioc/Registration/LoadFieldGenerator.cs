namespace Labo.Common.Ioc.Registration
{
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class LoadFieldGenerator : BaseInstanceGenerator
    {
        private readonly FieldGenerator m_FieldGenerator;

        public LoadFieldGenerator(FieldGenerator fieldGenerator)
            : base(fieldGenerator.Type)
        {
            m_FieldGenerator = fieldGenerator;
        }

        public override void Generate(ILGenerator generator)
        {
            FieldBuilder fieldBuilder = m_FieldGenerator.FieldBuilder;
            if (m_FieldGenerator.IsStatic)
            {
                EmitHelper.Ldsfld(generator, fieldBuilder);
            }
            else
            {
                EmitHelper.Ldfld(generator, fieldBuilder);
            }
        }
    }
}