namespace Labo.Common.Patterns
{
    public interface IFactoryInstanceCreator<out TInstance>
    {
        TInstance CreateInstance();
    }
}