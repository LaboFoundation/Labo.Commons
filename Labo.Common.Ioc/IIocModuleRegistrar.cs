namespace Labo.Common.Ioc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIocModuleRegistrar : IIocContainer
    {
        /// <summary>
        /// Registers the module.
        /// </summary>
        /// <param name="iocModule">The module.</param>
        void RegisterModule(IIocModule iocModule);
    }
}