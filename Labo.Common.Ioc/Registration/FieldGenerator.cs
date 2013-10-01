namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class FieldGenerator : BaseInstanceGenerator
    {
        private readonly string m_FieldName;

        private readonly IInstanceGenerator m_InstanceGenerator;

        private readonly FieldAttributes m_FieldAttributes;

        public FieldAttributes FieldAttributes
        {
            get
            {
                return m_FieldAttributes;
            }
        }

        public IInstanceGenerator InstanceGenerator
        {
            get
            {
                return m_InstanceGenerator;
            }
        }

        public ClassGenerator Owner { get; internal set; }

        public bool IsStatic { get{ return (FieldAttributes & FieldAttributes.Static) == FieldAttributes.Static; } }

        private readonly Lazy<FieldBuilder> m_FieldBuilder;

        public FieldBuilder FieldBuilder
        {
            get
            {
                return m_FieldBuilder.Value;
            }
        }

        public FieldGenerator(ClassGenerator owner, string fieldName, IInstanceGenerator instanceBuilder, FieldAttributes fieldAttributes = FieldAttributes.Private)
            : base(instanceBuilder.Type)
        {
            Owner = owner;
            m_FieldName = fieldName;
            m_InstanceGenerator = instanceBuilder;
            m_FieldAttributes = fieldAttributes;

            m_FieldBuilder = new Lazy<FieldBuilder>(() => Owner.TypeBuilder.DefineField(m_FieldName, m_InstanceGenerator.Type, m_FieldAttributes));
        }

        public override void Generate(ILGenerator generator)
        {
            if (m_InstanceGenerator != null)
            {
                m_InstanceGenerator.Generate(generator);                
            }

            FieldBuilder fieldBuilder = m_FieldBuilder.Value;
            if (IsStatic)
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