using System;
using System.IO;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Infrastructure.CrossCuttingConcerns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CleanTemplate.Infrastructure.Persistence
{
    // ApplicationDbContextFactory is used to create an instance of DbContext when we run dotnet-ef commands.
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseMySql(
                    connectionString,
                    sqlOptions => sqlOptions
                        .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                        .ServerVersion(new Version(10, 4, 0), ServerType.MySql)));
            services.AddTransient<ICurrentUserService, NullCurrentUserService>();
            services.AddTransient<IDateTime, DateTimeProvider>();

            var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            return context;
        }
    }
}
