namespace Labo.Common.Ioc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIocModule
    {
        /// <summary>
        /// Configures the specified registry.
        /// </summary>
        /// <param name="registry">The registry.</param>
        void Configure(IIocContainer registry);
    }
}