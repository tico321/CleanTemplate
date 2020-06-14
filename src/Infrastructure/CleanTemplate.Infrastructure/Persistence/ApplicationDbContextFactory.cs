using System.IO;
using CleanTemplate.Application.CrossCuttingConcerns;
using CleanTemplate.Infrastructure.CrossCuttingConcerns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            services.AddTransient<ICurrentUserService, NullCurrentUserService>();
            services.AddTransient<IDateTime, DateTimeProvider>();

            var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            return context;
        }
    }
}
