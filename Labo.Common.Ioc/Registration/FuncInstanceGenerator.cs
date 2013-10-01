namespace Labo.Common.Ioc.Registration
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    public sealed class FuncInstanceGenerator<T> : BaseInstanceGenerator
    {
        private readonly Func<T> m_InstanceCreator;

        private readonly ClassGenerator m_Owner;

        public FuncInstanceGenerator(ClassGenerator owner, Func<T> instanceCreator)
            : base(typeof(T))
        {
            if (instanceCreator == null)
            {
                throw new ArgumentNullException("instanceCreator");
            }

            m_InstanceCreator = instanceCreator;
            m_Owner = owner;
        }

        public override void Generate(ILGenerator generator)
        {
            DynamicMethod dynamicMethod = DynamicMethodHelper.CreateDynamicMethod("DynamicMethod", MethodAttributes.Static | MethodAttributes.Public, typeof(object), new[] { typeof(Func<object>) }, m_Owner.TypeBuilder.Module);
            ILGenerator dynamicMethodGenerator = dynamicMethod.GetILGenerator();
            
            EmitHelper.Ldarg(dynamicMethodGenerator, 0);
            MethodInfo invokeMethodInfo = typeof(Func<T>).GetMethod("Invoke");
            EmitHelper.Call(dynamicMethodGenerator, invokeMethodInfo);
            EmitHelper.Ret(dynamicMethodGenerator);

            Func<Func<T>, T> method = (Func<Func<T>, T>)dynamicMethod.CreateDelegate(typeof(Func<Func<T>, T>));
            object instance = method(m_InstanceCreator);
        }
    }
}