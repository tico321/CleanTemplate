using CleanTemplate.Application;
using CleanTemplate.GraphQL.Filters;
using CleanTemplate.GraphQL.Middleware;
using CleanTemplate.Infrastructure;
using HotChocolate;
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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddErrorFilter<AppExceptionFilter>();
            services.AddGraphQL(new SchemaBuilder()
                .AddQueryType<Query>()
                .Create());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            // Logging
            app.UseCorrelationId();
            // Collects information from a request and logs one event instead of many
            app.UseSerilogRequestLogging();

            app
                .UseRouting()
                .UseGraphQL();
        }
    }
}
