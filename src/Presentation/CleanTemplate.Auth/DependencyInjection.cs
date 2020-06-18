using System;
using System.Reflection;
using CleanTemplate.Auth.Application.Model;
using CleanTemplate.Auth.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CleanTemplate.Auth
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIISOptions(this IServiceCollection services)
        {
            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AuthDbContext>(options =>
                options.UseMySql(
                    connectionString,
                    sqlOptions => sqlOptions
                        .MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)
                        .ServerVersion(new Version(10, 4, 0), ServerType.MySql)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var serverUri = Environment.GetEnvironmentVariable("AUTHSERVER_AUTHORITY") ??
                            throw new Exception("AUTHORITY_URL is not defined");

            var builder = services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    // We need to set the Issuer and the Public Origin so when other service
                    // makes a request the Authority Uri that they will use matches this Server Uri.
                    options.IssuerUri = serverUri;
                    options.PublicOrigin = serverUri;
                })
                // to use sql server instead review https://identityserver4.readthedocs.io/en/latest/quickstarts/5_entityframework.html
                // AddConfigurationStore can be found in the IdentityServer4.EntityFramework package.
                .AddConfigurationStore(options =>
                    options.ConfigureDbContext = b => b
                        .UseMySql(connectionString, o => o.MigrationsAssembly(migrationsAssembly)
                            .ServerVersion(new Version(10, 4, 0), ServerType.MySql)))
                .AddOperationalStore(options =>
                    options.ConfigureDbContext = b => b
                        .UseMySql(connectionString, o => o.MigrationsAssembly(migrationsAssembly)
                            .ServerVersion(new Version(10, 4, 0), ServerType.MySql)))
                .AddAspNetIdentity<ApplicationUser>();

            // Add sign-in credentials
            // NOTE: This are not production ready, you should register here trusted credentials.
            builder.AddDeveloperSigningCredential();

            services.AddAuthentication();

            return services;
        }
    }
}
