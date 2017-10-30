using System;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Common.IdentityServer
{
    public class IdentityServerAuthenticationOptions:AuthenticationSchemeOptions
    {
       

        /// <summary>
        /// Base-address of the token issuer
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Specifies whether HTTPS is required for the discovery endpoint
        /// </summary>
        public bool RequireHttpsMetadata { get; set; } = true;

        /// <summary>
        /// Specifies which token types are supported (JWT, reference or both)
        /// </summary>
        //public SupportedTokens SupportedTokens { get; set; } = SupportedTokens.Both;

        /// <summary>
        /// Callback to retrieve token from incoming request
        /// </summary>
        //public Func<HttpRequest, string> TokenRetriever { get; set; } = TokenRetrieval.FromAuthorizationHeader();

        /// <summary>
        /// Name of the API resource used for authentication against introspection endpoint
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// Secret used for authentication against introspection endpoint
        /// </summary>
        public string ApiSecret { get; set; }

        public HttpMessageHandler Hander { get; set; }
    }
}