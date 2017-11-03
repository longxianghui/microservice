using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Identity
{
    public class Config
    {
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
                {
                    ApiSecrets = {new Secret("secret".Sha256())},
                    UserClaims = new List<string>{"role"}
                }
            };
        }

        // clients want to access resources (aka scopes)
        public IEnumerable<Client> GetClients()
        {
            var accessTokenLifetime = int.Parse(_configuration.GetConnectionString("AccessTokenLifetime"));
            return new List<Client>
            {
                new Client
                {
                    ClientId = "ro.client",
                    ClientSecrets =
                    {
                        new Secret("cb592770-f241-4079-8140-b7b7f2c2b132".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = accessTokenLifetime,
                    AllowedScopes =
                    {
                        "api1"
                    },
                    AccessTokenType =AccessTokenType.Reference
                }
            };
        }
    }
}