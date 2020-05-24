using System.Reflection;
using AutoMapper;
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
            // Registers all the classes that implement IRequestHandler
            services.AddMediatR(Assembly.GetExecutingAssembly());
            // Registers all the profiles under CrossCuttingConcerns/IMapFrom.cs
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggerBehavior<,>));
            services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            return services;
        }
    }
}
