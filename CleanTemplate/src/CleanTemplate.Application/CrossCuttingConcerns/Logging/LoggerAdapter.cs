using System;
using Microsoft.Extensions.Logging;

namespace CleanTemplate.Application.CrossCuttingConcerns.Logging
{
    // Based on https://ardalis.com/testing-logging-in-aspnet-core
    // The main motivation is to have an interface that can be easily tested
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            this._logger = logger;
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            _logger.LogError(ex, message, args);
        }
    }
}
