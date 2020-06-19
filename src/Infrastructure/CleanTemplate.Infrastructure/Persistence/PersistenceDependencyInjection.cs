using System;
using CleanTemplate.Application.CrossCuttingConcerns.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CleanTemplate.Infrastructure.Persistence
{
    public static class PersistenceDependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseMySql(
                        connectionString,
                        sqlOptions => sqlOptions
                            .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                            .ServerVersion(new Version(10, 4, 0), ServerType.MySql)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        }
    }
}
