using System.Reflection;
using CleanTemplate.Application.CrossCuttingConcerns.Behaviors;
using CleanTemplate.Application.CrossCuttingConcerns.Logging;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggerBehavior<,>));
            services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            return services;
        }
    }
}