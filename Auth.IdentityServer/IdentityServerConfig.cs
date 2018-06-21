using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api","My Api")
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId="clientId",
                    AllowedGrantTypes = {GrantType.ClientCredentials },
                    ClientSecrets={new Secret("secret".Sha256()) },
                    AllowedScopes={IdentityServerConstants.StandardScopes.OpenId, "api"},
                    RedirectUris={"https://www.getpostman.com/oauth2/callback" }
                }
            };
        }
    }
}
