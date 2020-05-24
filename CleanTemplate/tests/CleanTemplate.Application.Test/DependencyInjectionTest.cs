using System;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Application.CrossCuttingConcerns.Logging;
using CleanTemplate.Application.Test.TestHelpers;
using CleanTemplate.Application.Todos.Commands.Create;
using CleanTemplate.Application.Todos.Queries.GetAll;
using FakeItEasy;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CleanTemplate.Application.Test
{
    public class DependencyInjectionTest
    {
        [Fact]
        public void RegistersDependencies()
        {
            var services = new ServiceCollection();
            // Register types that will be registered in CleanTemplate.API.IntegrationTest
            services.AddTransient(typeof(ILogger<>), typeof(FakeLogger<>));
            services.AddTransient<ICurrentUserService, FakeUserService>();
            services.AddTransient(serviceProvider => A.Fake<IApplicationDbContext>());

            services.AddApplication();
            var provider = services.BuildServiceProvider();

            Assert.NotNull(provider.GetService<IPipelineBehavior<GetAllTodosQuery, TodoListVm>>());
            Assert.NotNull(provider.GetService<ILoggerAdapter<string>>());
            Assert.NotNull(provider.GetService<IRequestHandler<GetAllTodosQuery, TodoListVm>>());
            Assert.NotNull(provider.GetService<IRequestHandler<CreateTodoCommand, long>>());
        }

        private class FakeLogger<T> : ILogger<T>
        {
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                throw new NotImplementedException();
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
    }
}
