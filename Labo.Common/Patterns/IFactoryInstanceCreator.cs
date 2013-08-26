namespace Labo.Common.Patterns
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    public interface IFactoryInstanceCreator<out TInstance>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        TInstance CreateInstance();
    }
}