// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace CleanTemplate.Auth.Persistence.Seed
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[] { new IdentityResources.OpenId(), new IdentityResources.Profile(), new IdentityResources.Email() };


        public static IEnumerable<ApiResource> Apis =>
            new[] { new ApiResource("todo_api", "Todo Rest API Access"), new ApiResource("todo_graphql", "Todo GraphQL API Access") };


        public static IEnumerable<Client> Clients =>
            new[]
            {
                // ClientCredentials Sample
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    // ClientCredentials doesn't have interactive users, it's usually used for microservices (machine to machine communication)
                    // You can think of the ClientId and the ClientSecret as the login and password for your application itself.
                    // It identifies your APPLICATION to the identity server so that it knows which APPLICATION is trying to connect to it.
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // scopes that client has access to
                    AllowedScopes = { "todo_api", "todo_graphql" }
                },

                // Javascript Client
                new Client
                {
                    ClientId = "ReactClient",
                    ClientName = "React Client",

                    // The Authorization Code grant type is used by confidential and public clients to exchange an authorization code for an access token.
                    // The Authorization Code grant type is used by web and mobile apps. It differs from most of the other grant types by first requiring the app launch a browser to begin the flow. At a high level, the flow has the following steps:
                    //     The application opens a browser to send the user to the OAuth server
                    //     The user sees the authorization prompt and approves the app’s request
                    //     The user is redirected back to the application with an authorization code in the query string
                    //     The application exchanges the authorization code for an access token
                    // After the user returns to the client via the redirect URL, the application will get the authorization code from the URL and use it to request an access token.
                    // It is recommended that all clients use the PKCE extension with this flow as well to provide better security.
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true, // an extension to the Authorization Code flow to prevent certain attacks
                    RequireClientSecret = false, // as it's a JS client we don't want to send the secret to the browser
                    AllowOfflineAccess = true, // so we get a refresh token
                    AccessTokenType = AccessTokenType.Jwt, // self-contained access token http://docs.identityserver.io/en/latest/topics/reference_tokens.html

                    // UI route to redirect to after login
                    RedirectUris = { "http://localhost:3000/callback" },
                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:3000/" },
                    // So the Javascript client can query this Server
                    AllowedCorsOrigins = { "http://localhost:3000", "https://localhost:3001" },


                    // IdentityServer will return two tokens: the identity token containing the information about the authentication and session,
                    // and the access token to access APIs on behalf of the logged on user.
                    AllowedScopes = new List<string>
                    {
                        // Identity resources
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        // API resources
                        "todo_api",
                        "todo_graphql"
                    }
                }
            };
    }
}
