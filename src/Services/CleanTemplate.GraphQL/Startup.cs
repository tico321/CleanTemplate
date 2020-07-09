using CleanTemplate.Application;
using CleanTemplate.Infrastructure;
using CleanTemplate.Infrastructure.Logging;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CleanTemplate.GraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddApplication()
                .AddInfrastructure(Configuration)
                .AddCors()
                .AddAuthServer()
                .AddGraphQlApi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            // Logging
            app
                // Adds a unique Id per request, if the header CorrelationId is present it will use it instead.
                .UseCorrelationId()
                // Collects information from a request and logs one event instead of many
                .UseSerilogRequestLogging();

            // https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1
            app.UseCors(
                builder =>
                {
                    builder
                        .WithOrigins(Configuration.GetSection("AllowedCors").Get<string[]>())
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

            // Use authentication and authorization middleware
            app.UseAuth();

            // GraphQL
            app.UseGraphQL();
        }
    }
}
