namespace Labo.Common.Ioc.Registration
{
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class StoreFieldGenerator : BaseEmitILGenerator
    {
        private readonly DefineFieldGenerator m_FieldGenerator;
        private readonly BaseEmitILGenerator m_Value;

        public StoreFieldGenerator(DefineFieldGenerator fieldGenerator, BaseEmitILGenerator value)
            : base(fieldGenerator.Type)
        {
            m_FieldGenerator = fieldGenerator;
            m_Value = value;
        }

        public override void Generate(ILGenerator generator)
        {
            FieldInfo fieldInfo = m_FieldGenerator.FieldInfo;
            if (m_FieldGenerator.IsStatic)
            {
                EmitHelper.Stsfld(generator, fieldInfo);
            }
            else
            {
                EmitHelper.Ldarg(generator, 0);
                m_Value.Generate(generator);
                EmitHelper.Stfld(generator, fieldInfo);
            }
        }
    }
}