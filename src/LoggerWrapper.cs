using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace RecipeManager.API
{
    [ExcludeFromCodeCoverage]
    public class LoggerWrapper : ILoggerWrapper
    {
        private readonly ILogger<LoggerWrapper> _logger;

        public LoggerWrapper(ILogger<LoggerWrapper> logger)
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
