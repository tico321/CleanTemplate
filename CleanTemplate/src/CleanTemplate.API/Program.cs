using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NpgsqlTypes;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.PostgreSQL;

namespace CleanTemplate.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // We initialize logging before the host so we have logging even if startup fails
            InitLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseSerilog();
                });
        }

        private static void InitLogger()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Column writers for PostgreSQL sink https://github.com/b00ted/serilog-sinks-postgresql
            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                {"Message", new RenderedMessageColumnWriter()},
                {"Level", new LevelColumnWriter(renderAsText: true, NpgsqlDbType.Varchar)},
                {"TimeStamp", new TimestampColumnWriter()},
                {"Exception", new ExceptionColumnWriter()},
                {"MachineName", new SinglePropertyColumnWriter("MachineName")},
                {"CorrelationId", new SinglePropertyColumnWriter("CorrelationId")},
                {"UserId", new SinglePropertyColumnWriter("UserId")}
            };
            Log.Logger = new LoggerConfiguration()
                .Enrich
                .FromLogContext() //https://github.com/serilog/serilog/wiki/Enrichment used for the CorrelationIdMiddleware
                .Enrich.WithMachineName() // https://github.com/saleem-mirza/serilog-enrichers-context/wiki
                .WriteTo.PostgreSql(
                    connectionString,
                    "ApplicationLog",
                    columnWriters,
                    needAutoCreateTable: true
                    //useCopy: true,
                    //batchSizeLimit: 40,
                    //period: new TimeSpan(0, 0, 10)
                )
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
#if DEBUG
            // This will break automatically if Serilog throws an exception.
            SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                Debugger.Break();
            });
#endif
        }
    }
}
