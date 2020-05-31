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
            AddGraphQLAPI(services);
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

        // Adds Hotcholate GrahpQL https://hotchocolate.io/docs/introduction
        private void AddGraphQLAPI(IServiceCollection services)
        {
            // Add GraphQL exception filter
            services.AddErrorFilter<AppExceptionFilter>();
            // if you want to read more about DataLoader in general, you can head over to Facebook's GitHub repository https://github.com/graphql/dataloader
            // DataLoader solves the called n+1 problem for GraphQL.
            services.AddDataLoaderRegistry(); //repository https://github.com/ChilliCream/greendonut
            // Add GraphQL Schema
            services.AddGraphQL(new SchemaBuilder()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .Create());
        }
    }
}
