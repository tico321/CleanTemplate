using System;
using CleanTemplate.Application.CrossCuttingConcerns.Logging;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CleanTemplate.Application.Test.CrossCuttingConcerns.Logging
{
    public class LoggerAdapterTest
    {
        private class FakeILogger<T> : ILogger<T>
        {
            public LogLevel CalledLogLevel { get; set; }
            public Exception CalledException { get; set; }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter)
            {
                CalledLogLevel = logLevel;
                CalledException = exception;
            }


            public bool IsEnabled(LogLevel logLevel)
            {
                throw new NotImplementedException();
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void LogError()
        {
            var logger = new FakeILogger<string>();
            var loggerAdapter = new LoggerAdapter<string>(logger);

            loggerAdapter.LogError(new Exception("msg"), "msg");

            Assert.Equal(LogLevel.Error, logger.CalledLogLevel);
            Assert.NotNull(logger.CalledException);
        }

        [Fact]
        public void LogInformation()
        {
            var logger = new FakeILogger<string>();
            var loggerAdapter = new LoggerAdapter<string>(logger);

            loggerAdapter.LogInformation("msg");

            Assert.Equal(LogLevel.Information, logger.CalledLogLevel);
            Assert.Null(logger.CalledException);
        }
    }
}
