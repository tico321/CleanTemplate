using CleanTemplate.Application.CrossCuttingConcerns;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.Infrastructure.Persistence
{
  public static class PersistenceDependencyInjection
  {
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
      var conn = configuration.GetConnectionString("DefaultConnection");
      services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                conn,
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

      services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
    }
  }
}
