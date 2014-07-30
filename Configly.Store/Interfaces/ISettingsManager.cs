namespace Configly.Store.Interfaces
{
    public interface ISettingsManager<T>
    {
        T Get();
        void Set(object setting);
    }
}
