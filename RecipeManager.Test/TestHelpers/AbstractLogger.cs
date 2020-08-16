﻿using System;
using Microsoft.Extensions.Logging;

namespace RecipeManager.Test.TestHelpers
{
    public abstract class AbstractLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state)
            => throw new NotImplementedException();

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            => Log(logLevel, exception, formatter(state, exception));

        protected abstract void Log(LogLevel logLevel, Exception ex, string information);
    }
}
