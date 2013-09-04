namespace Labo.Common.Ioc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIocModuleModule : IIocContainer
    {
        /// <summary>
        /// Registers the module.
        /// </summary>
        /// <param name="iocModule">The module.</param>
        void RegisterModule(IIocModule iocModule);
    }
}