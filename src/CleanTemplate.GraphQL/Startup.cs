using CleanTemplate.Application;
using CleanTemplate.GraphQL.Middleware;
using CleanTemplate.Infrastructure;
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
                .AddAuthServer()
                .AddGraphQlApi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            // Logging
            app
                // Adds a unique Id per request, if the header CorrelationId is present it will use it instead.
                .UseCorrelationId()
                // Collects information from a request and logs one event instead of many
                .UseSerilogRequestLogging();

            // Auth
            app
                // UseAuthentication adds the authentication middleware to the pipeline so authentication is performed on every call into the host.
                .UseAuthentication()
                // UseAuthorization adds the authorization middleware to make sure our API cannot be accessed by anonymous clients.
                .UseAuthorization();

            // GraphQL
            app.UseGraphQL();
        }
    }
}
