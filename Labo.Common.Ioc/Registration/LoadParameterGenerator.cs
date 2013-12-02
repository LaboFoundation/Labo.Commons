namespace Labo.Common.Ioc.Registration
{
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class LoadParameterGenerator : BaseEmitILGenerator
    {
        private readonly DefineParameterGenerator m_DefineParameterGenerator;

        public LoadParameterGenerator(DefineParameterGenerator defineParameterGenerator)
            : base(defineParameterGenerator.Type)
        {
            m_DefineParameterGenerator = defineParameterGenerator;
        }

        public override void Generate(ILGenerator generator)
        {
            EmitHelper.Ldarg(generator, m_DefineParameterGenerator.MethodGenerator.IsStatic ? m_DefineParameterGenerator.Position : m_DefineParameterGenerator.Position + 1);
        }
    }
}