namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public sealed class DefineFieldGenerator : BaseEmitILGenerator
    {
        private readonly string m_FieldName;

        private FieldInfo m_FieldInfo;

        private readonly FieldAttributes m_FieldAttributes;

        public FieldAttributes FieldAttributes
        {
            get
            {
                return m_FieldAttributes;
            }
        }

        public TypeBuilder Owner { get; internal set; }

        public bool IsStatic { get { return (FieldAttributes & FieldAttributes.Static) == FieldAttributes.Static; } }

        public FieldInfo FieldInfo
        {
            get
            {
                return m_FieldInfo;
            }
        }

        public DefineFieldGenerator(TypeBuilder owner, Type type, string fieldName, FieldAttributes fieldAttributes = FieldAttributes.Private)
            : base(type)
        {
            Owner = owner;
            m_FieldName = fieldName;
            m_FieldAttributes = fieldAttributes;
        }

        public override void Generate(ILGenerator generator)
        {
            m_FieldInfo = Owner.DefineField(m_FieldName, Type, m_FieldAttributes);
        }
    }
}