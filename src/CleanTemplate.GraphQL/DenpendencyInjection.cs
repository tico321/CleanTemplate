using System;
using CleanTemplate.GraphQL.Filters;
using HotChocolate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace CleanTemplate.GraphQL
{
    public static class DenpendencyInjection
    {
        public static IServiceCollection AddAuthServer(this IServiceCollection services)
        {
            // Uncomment this to see Identiy error details https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/PII
            IdentityModelEventSource.ShowPII = true;

            var authority = Environment.GetEnvironmentVariable("AUTHSERVER_AUTHORITY");
            // We use package Microsoft.AspNetCore.Authentication.JwtBearer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // Adds the authentication services to DI
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => // configures Bearer as the default scheme
                {
                    options.Authority = authority; // AuthServer URL
                    options.Audience = "todo_graphql"; // clients need permissions to access this resource
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }

        // Adds Hotcholate GrahpQL https://hotchocolate.io/docs/introduction
        public static IServiceCollection AddGraphQlApi(this IServiceCollection services)
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

            return services;
        }
    }
}
