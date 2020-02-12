namespace TreeLine.Sagas
{
    public interface ILoggerAdapter<T>
    {
        void LogWarning(string message);
    }
}
