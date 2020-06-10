using System;
using CleanTemplate.GraphQL.Filters;
using HotChocolate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTemplate.GraphQL
{
    public static class DenpendencyInjection
    {
        public static IServiceCollection AddAuthServer(this IServiceCollection services)
        {
            // Uncomment this to see Identiy error details https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/PII
            // IdentityModelEventSource.ShowPII = true;

            var authority = Environment.GetEnvironmentVariable("AUTHSERVER_AUTHORITY");
            //var secretKey = Environment.GetEnvironmentVariable("AUTHSERVER_SECRET_KEY");
            if (string.IsNullOrEmpty(authority))
            {
                throw new Exception("Missing AuthServer configuration.");
            }

            // We use package Microsoft.AspNetCore.Authentication.JwtBearer
            services
                // Adds the authentication services to DI
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // configures Bearer as the default scheme
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Authority = authority; // AuthServer URL
                        options.Audience = "todo_graphql"; // clients need this scope to access the API
                        options.RequireHttpsMetadata = false; // So we can communicate through http instead of https
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
