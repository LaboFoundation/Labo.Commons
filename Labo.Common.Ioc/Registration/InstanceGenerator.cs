namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class InstanceGenerator : BaseInstanceGenerator
    {
        private readonly ConstructorInfo m_Constructor;

        private readonly List<IInstanceGenerator> m_Parameters;

        public InstanceGenerator(Type type, ConstructorInfo constructor, params IInstanceGenerator[] parameters)
            : base(type)
        {
            this.m_Constructor = constructor;

            this.m_Parameters = new List<IInstanceGenerator>(parameters.Length);
            for (int i = 0; i < parameters.Length; i++)
            {
                IInstanceGenerator instanceGenerator = parameters[i];
                this.m_Parameters.Add(instanceGenerator);
            }
        }

        public override void Generate(ILGenerator generator)
        {
            for (int i = 0; i < this.m_Parameters.Count; i++)
            {
                IInstanceGenerator instanceGenerator = this.m_Parameters[i];
                instanceGenerator.Generate(generator);
            }

            EmitHelper.Newobj(generator, this.m_Constructor);
        }
    }
}