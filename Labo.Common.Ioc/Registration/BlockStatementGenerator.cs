namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Reflection.Emit;

    public sealed class BlockStatementGenerator : BaseEmitILGenerator
    {
        private readonly BaseEmitILGenerator[] m_Statements;

        public BlockStatementGenerator(Type type, params BaseEmitILGenerator[] statements)
            : base(type)
        {
            m_Statements = statements;
        }

        public override void Generate(ILGenerator generator)
        {
            for (int i = 0; i < m_Statements.Length; i++)
            {
                BaseEmitILGenerator baseEmitIlGenerator = m_Statements[i];
                baseEmitIlGenerator.Generate(generator);
            }
        }
    }
}