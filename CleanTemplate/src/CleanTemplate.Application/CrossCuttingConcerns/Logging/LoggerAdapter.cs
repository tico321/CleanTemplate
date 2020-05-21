using System;
using Microsoft.Extensions.Logging;

namespace CleanTemplate.Application.CrossCuttingConcerns.Logging
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public void LogInformation(string message, params object[] args)
        {
            logger.LogInformation(message, args);
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            logger.LogError(ex, message, args);
        }
    }
}