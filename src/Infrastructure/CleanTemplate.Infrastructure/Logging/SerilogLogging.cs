using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Debugging;
using Serilog.Sinks.MariaDB;
using Serilog.Sinks.MariaDB.Extensions;
using Serilog.Sinks.SystemConsole.Themes;

namespace CleanTemplate.Infrastructure.Logging
{
    public static class SerilogLogging
    {
        public static LoggerConfiguration InitLogger(WebHostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
            var logger = loggerConfiguration
                .Enrich
                //https://github.com/serilog/serilog/wiki/Enrichment used to add custom enrichers like int the CorrelationIdMiddleware
                .FromLogContext()
                // https://github.com/saleem-mirza/serilog-enrichers-context/wiki
                // .Enrich.WithMachineName()
                .WriteTo.MariaDB(
                    connectionString,
                    tableName: "Logs",
                    autoCreateTable: true,
                    options: new MariaDBSinkOptions
                    {
                        PropertiesToColumnsMapping = new Dictionary<string, string>
                        {
                            { "Message", "Message" },
                            { "Level", "Level" },
                            { "Timestamp", "Timestamp" },
                            { "Exception", "Exception" },
                            { "CorrelationId", "CorrelationId" },
                            { "UserId", "UserId" },
                            { "Properties", "Properties" },
                            //{ "MachineName", "MachineName" },
                            { "MessageTemplate", null }
                        },
                        TimestampInUtc = true,
                        ExcludePropertiesWithDedicatedColumn = true
                    })
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Literate)
                .ReadFrom.Configuration(context.Configuration);
#if DEBUG
            // This will break automatically if Serilog throws an exception.
            SelfLog.Enable(
                msg =>
                {
                    Debug.Print(msg);
                    Debugger.Break();
                });
#endif
            return logger;
        }
    }
}
