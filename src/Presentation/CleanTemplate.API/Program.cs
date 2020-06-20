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
            try
            {
                var host = CreateHostBuilder(args).Build();
                host.Run();
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
                            .UseSerilog((context, configuration) => SerilogLogging.InitLogger(context, configuration));
                    });
        }
    }
#pragma warning restore CS1591
}
