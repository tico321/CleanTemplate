using System;

namespace CleanTemplate.Application.CrossCuttingConcerns.Logging
{
    // Based on https://ardalis.com/testing-logging-in-aspnet-core
    // The main motivation is to have an interface that can be easily tested.
    public interface ILoggerAdapter<T>
    {
        void LogInformation(string message, params object[] args);
        void LogError(Exception ex, string message, params object[] args);
    }
}
