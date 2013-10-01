namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Reflection.Emit;

    public interface IInstanceGenerator
    {
        void Generate(ILGenerator generator);

        Type Type { get; }

        CastInstanceGenerator Cast(Type type);
    }
}