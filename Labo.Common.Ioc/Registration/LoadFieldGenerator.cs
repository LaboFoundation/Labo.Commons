namespace Labo.Common.Ioc.Registration
{
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class LoadFieldGenerator : BaseEmitILGenerator
    {
        private readonly DefineFieldGenerator m_FieldGenerator;

        public LoadFieldGenerator(DefineFieldGenerator fieldGenerator)
            : base(fieldGenerator.Type)
        {
            m_FieldGenerator = fieldGenerator;
        }

        public override void Generate(ILGenerator generator)
        {
            FieldInfo fieldInfo = m_FieldGenerator.FieldInfo;
            if (m_FieldGenerator.IsStatic)
            {
                EmitHelper.Ldsfld(generator, fieldInfo);
            }
            else
            {
                EmitHelper.Ldarg(generator, 0);
                EmitHelper.Ldfld(generator, fieldInfo);
            }
        }
    }
}