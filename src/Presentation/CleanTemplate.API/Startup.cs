using AutoWrapper;
using CleanTemplate.API.Filters;
using CleanTemplate.API.Middleware;
using CleanTemplate.Application;
using CleanTemplate.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddControllers(options =>
            {
                options.Filters.Add(new AppExceptionFilter());
            });
            services.AddApplication();
            services.AddInfrastructure(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Logging
            app.UseCorrelationId();
            // Collects information from a request and logs one event instead of many
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            // Problem Details support added with AutoWrapper https://github.com/proudmonkey/AutoWrapper
            app.UseApiResponseAndExceptionWrapper();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
