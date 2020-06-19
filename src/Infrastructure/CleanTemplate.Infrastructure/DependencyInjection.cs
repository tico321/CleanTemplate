using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Infrastructure.CrossCuttingConcerns;
using CleanTemplate.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            services.AddTransient<IDateTime, DateTimeProvider>();
            services.AddTransient<ICurrentUserService, NullCurrentUserService>();

            return services;
        }
    }
}
