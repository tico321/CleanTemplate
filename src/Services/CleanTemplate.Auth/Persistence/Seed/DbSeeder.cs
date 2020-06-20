using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanTemplate.Auth.Application.Model;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;

namespace CleanTemplate.Auth.Persistence.Seed
{
    public class DbSeeder
    {
        public static async Task SeedClients(ConfigurationDbContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.Ids)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.Apis)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var alice = userManager.FindByNameAsync("alice").Result;
            if (alice == null)
            {
                alice = new ApplicationUser { UserName = "alice" };
                await userManager.CreateAsync(alice, "Pass123$");
                await userManager.AddClaimsAsync(
                    alice,
                    new[]
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(
                            JwtClaimTypes.Address,
                            @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                            IdentityServerConstants.ClaimValueTypes.Json)
                    });
            }

            var bob = userManager.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser { UserName = "bob" };
                await userManager.CreateAsync(bob, "Pass123$");
                await userManager.AddClaimsAsync(
                    bob,
                    new[]
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(
                            JwtClaimTypes.Address,
                            @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                            IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("location", "somewhere")
                    });
            }
        }
    }
}
