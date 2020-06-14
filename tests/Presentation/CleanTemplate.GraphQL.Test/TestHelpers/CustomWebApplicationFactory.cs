using System;
using System.Linq;
using CleanTemplate.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.GraphQL.Test.TestHelpers
{
    // Note that CustomWebApplicationFactory is shared among Test classes
    // For more information visit https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services =>
            {
                SetTestDbContext(services);
                SeedDb(services);
            });
        }

        /// <summary>
        ///     Creates a new WebApplicationFactory with a fresh DB recently seeded.
        ///     For this to work properly parallelization in xunit needs to be disabled
        ///     https://stackoverflow.com/a/61122438/8765790
        /// </summary>
        /// <returns>A new HttpClient </returns>
        public WebApplicationFactory<TStartup> Reset(Action<ApplicationDbContext> seeder = null)
        {
            return WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    ResetDb(services, seeder);
                });
            });
        }

        private void SetTestDbContext(IServiceCollection services)
        {
            // Remove the app's ApplicationDbContext registration.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add a database context using an in-memory database for testing.
            services.AddDbContext<ApplicationDbContext>(
                options => { options.UseInMemoryDatabase(new Guid().ToString()); });
        }

        private void SeedDb(IServiceCollection services)
        {
            // Build the service provider
            var sp = services.BuildServiceProvider();
            // Create a scope to obtain a reference to the database context (ApplicationDbContext).
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<ApplicationDbContext>();
            // Ensure the database is created.
            context.Database.EnsureCreated();

            try
            {
                Seeder.Seed(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while seeding the DB. Error: {0}", ex.Message);
            }
        }

        private void ResetDb(IServiceCollection services, Action<ApplicationDbContext> seeder = null)
        {
            seeder ??= Seeder.Seed;
            // Build the service provider
            var sp = services.BuildServiceProvider();
            // Create a scope to obtain a reference to the database context (ApplicationDbContext).
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<ApplicationDbContext>();

            try
            {
                Seeder.Reset(context);
                seeder(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while resetting the DB. Error: {0}", ex.Message);
            }
        }
    }
}
