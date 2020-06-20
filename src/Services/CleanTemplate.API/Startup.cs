using System;
using System.IO;
using System.Reflection;
using AutoWrapper;
using CleanTemplate.API.Filters;
using CleanTemplate.Application;
using CleanTemplate.Infrastructure;
using CleanTemplate.Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace CleanTemplate.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // .Net core healthchecks https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.1
            services.AddHealthChecks();

            services.AddControllers(
                options =>
                {
                    options.Filters.Add(new AppExceptionFilter());
                });

            // Swagger with Swashbuckle https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
            services.AddSwaggerGen(
                c =>
                {
                    // Define the version and additional Info
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });

                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                });

            // Project dependencies
            services
                .AddApplication()
                .AddInfrastructure(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Logging
            app
                // Adds a CorrelationId to every request
                .UseCorrelationId()
                // Collects information from a request and logs one event instead of many
                .UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            // Problem Details support added with AutoWrapper https://github.com/proudmonkey/AutoWrapper
            app.UseApiResponseAndExceptionWrapper();

            // Swagger
            app
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                .UseSwagger()
                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                .UseSwaggerUI(
                    c =>
                    {
                        // specifying the Swagger JSON endpoint.
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/health");
                });
        }
    }
}
