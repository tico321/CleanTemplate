using CleanTemplate.Application.CrossCuttingConcerns.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Application.CrossCuttingConcerns.Validation
{
    public static class ValidationDependencyInjection
    {
        public static IServiceCollection AddAssemblyValidators(this IServiceCollection services)
        {
            // Get all validators In Assembly
            var validators = AssemblyScanner.FindValidatorsInAssembly(typeof(RequestValidationBehavior<,>).Assembly);
            // Register all validators
            validators.ForEach(validator => services.AddTransient(validator.InterfaceType, validator.ValidatorType));

            return services;
        }
    }
}
