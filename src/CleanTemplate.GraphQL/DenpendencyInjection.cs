using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using CleanTemplate.GraphQL.Filters;
using HotChocolate;
using IdentityModel.Client;
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
            var secretKey = Environment.GetEnvironmentVariable("AUTHSERVER_SECRET_KEY");
            if (string.IsNullOrEmpty(authority) || string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("Missing AuthServer configuration.");
            }

            using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadWrite))
            {
                var certificate = new X509Certificate2("./CleanTemplateServerCert.pfx", secretKey, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                store.Add(certificate);
            }

            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync(authority).Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                throw new Exception();
            }

            //var certificate = new X509Certificate2("./CleanTemplateServerCert.pfx", secretKey, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            //X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            //store.Open(OpenFlags.ReadWrite);
            //var certificates = store.Certificates;
            //store.Add(certificate);
            //store.Close();

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
                        //options.RequireHttpsMetadata = false;
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
