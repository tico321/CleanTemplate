﻿using System.Reflection;
using AutoMapper;
using CleanTemplate.Application.CrossCuttingConcerns.Behaviors;
using CleanTemplate.Application.CrossCuttingConcerns.Logging;
using CleanTemplate.Application.CrossCuttingConcerns.Validation;
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
            // Registers all the validators that Extend AbstractValidator
            services.AddAssemblyValidators();
            // Register Mediatr Pipeline, the order or the pipeline depends on the order of registration
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggerBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            // Logging
            services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

            return services;
        }
    }
}
