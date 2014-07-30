namespace Configly.Store.Entities
{
    public interface ILogger
    {
        void Error(string message, object data = null);
        void Error(string format, params object[] args);
        void Information(string message, object data = null);
        void Information(string format, params object[] args);
        void Warning(string message, object data = null);
        void Warning(string format, params object[] args);
    }
}
