namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class CreateNewInstanceGenerator : BaseEmitILGenerator
    {
        private readonly ConstructorInfo m_Constructor;

        private readonly List<BaseEmitILGenerator> m_Parameters;

        public CreateNewInstanceGenerator(Type type, ConstructorInfo constructor, params BaseEmitILGenerator[] parameters)
            : base(type)
        {
            m_Constructor = constructor;

            m_Parameters = new List<BaseEmitILGenerator>(parameters.Length);
            for (int i = 0; i < parameters.Length; i++)
            {
                BaseEmitILGenerator instanceGenerator = parameters[i];
                m_Parameters.Add(instanceGenerator);
            }
        }

        public override void Generate(ILGenerator generator)
        {
            for (int i = 0; i < m_Parameters.Count; i++)
            {
                BaseEmitILGenerator instanceGenerator = m_Parameters[i];
                instanceGenerator.Generate(generator);
            }

            EmitHelper.Newobj(generator, m_Constructor);
        }
    }
}