using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdSrv
{
    public class Config
    {
        protected internal const string Favorittfarge = "favorittfarge";

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource(Favorittfarge, "Favorittfarge", new []{ Favorittfarge })
            };
        }
        
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("dotnet-api", ".NET API", new[] { JwtClaimTypes.Email, Favorittfarge })
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "js_oidc",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        Favorittfarge, 
                        "dotnet-api"
                    },
                    RedirectUris = { "http://localhost:7017/callback.html" },
                    AllowedCorsOrigins = { "http://localhost:7017/" }
                }
            };
        }
    }
}