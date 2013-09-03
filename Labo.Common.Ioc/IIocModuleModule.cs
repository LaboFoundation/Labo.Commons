namespace Labo.Common.Ioc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIocModuleModule : IIocContainer
    {
        void RegisterModule(IIocModule module);
    }
}