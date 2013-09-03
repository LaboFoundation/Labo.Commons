namespace Labo.Common.Ioc
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIocModule
    {
        void Configure(IIocContainer registry);
    }
}