using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdSrv
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
        
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("dotnet-api", ".NET API")
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
                    AllowedScopes = { "openid", "profile", "email", "dotnet-api" },
                    RedirectUris = { "http://localhost:7017/callback.html" },
                    AllowedCorsOrigins = { "http://localhost:7017/" }
                }
            };
        }
    }
}