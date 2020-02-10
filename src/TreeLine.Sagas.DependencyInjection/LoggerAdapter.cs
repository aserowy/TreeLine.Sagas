using Microsoft.Extensions.Logging;

namespace TreeLine.Sagas.DependencyInjection
{
    internal sealed class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }
    }
}
