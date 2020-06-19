using System;
using System.IO;
using CleanTemplate.Infrastructure.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Log = Serilog.Log;

namespace CleanTemplate.API
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args)
        {
            // We initialize logging before the host so we have logging even if startup fails
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                // Environment Variables will override default appsettings.json
                .AddEnvironmentVariables()
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            SerilogLogging.InitLogger(configuration, connectionString);

            try
            {
                var host = CreateHostBuilder(args).Build();
                Log.Information("Starting host...");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder
                            .UseStartup<Startup>()
                            .UseSerilog();
                    });
        }
    }
#pragma warning restore CS1591
}
