using System;
using Microsoft.Extensions.Logging;

namespace RecipeManager.API
{
    public class LoggerWrapper<T> : ILoggerWrapper
    {
        private readonly ILogger<T> _logger;

        public LoggerWrapper(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            _logger.LogError(ex, message, args);
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

    }

    public interface ILoggerWrapper
    {
        void LogError(Exception ex, string message, params object[] args);
        void LogInformation(string message);
    }
}
