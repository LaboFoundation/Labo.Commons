namespace Labo.Common.Ioc.Registration
{
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class MethodGenerator
    {
        private readonly string m_Name;

        private readonly MethodAttributes m_MethodAttributes;

        private readonly BaseEmitILGenerator m_ReturnValue;

        private readonly DefineParameterGenerator[] m_Parameters;

        public bool IsStatic { get { return (m_MethodAttributes & MethodAttributes.Static) == MethodAttributes.Static; } }

        public ClassGenerator Owner { get; internal set; }

        public MethodGenerator(ClassGenerator owner, string name, MethodAttributes methodAttributes, BaseEmitILGenerator returnValue = null, params DefineParameterGenerator[] parameters)
        {
            m_Name = name;
            m_MethodAttributes = methodAttributes;
            m_ReturnValue = returnValue;
            m_Parameters = parameters;
            Owner = owner;
        }

        public void Generate()
        {
            bool isVoid = m_ReturnValue == null;
            MethodBuilder methodBuilder = Owner.TypeBuilder.DefineMethod(m_Name, m_MethodAttributes, isVoid ? typeof(void) : m_ReturnValue.Type, m_Parameters.Select(x => x.Type).ToArray());
            ILGenerator methodGenerator = methodBuilder.GetILGenerator();
            for (int i = 0; i < m_Parameters.Length; i++)
            {
                DefineParameterGenerator parameter = m_Parameters[i];
                parameter.Position = i;
                parameter.MethodBuilder = methodBuilder;
                parameter.MethodGenerator = this;
                parameter.Generate(methodGenerator);
            }

            if (!isVoid)
            {
                m_ReturnValue.Generate(methodGenerator);
            }

            EmitHelper.Ret(methodGenerator);
        }
    }
}