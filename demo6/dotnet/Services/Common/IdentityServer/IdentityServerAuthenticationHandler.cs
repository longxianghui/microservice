using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.IdentityServer
{
    public class IdentityServerAuthenticationHandler : AuthenticationHandler<IdentityServerAuthenticationOptions>
    {
        public IdentityServerAuthenticationHandler(IOptionsMonitor<IdentityServerAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        string getToken(HttpRequest request, string scheme = "Bearer")
        {
            string str = request.Headers["Authorization"].FirstOrDefault<string>();
            if (string.IsNullOrEmpty(str))
                return (string) null;
            if (str.StartsWith(scheme + " ", StringComparison.OrdinalIgnoreCase))
                return str.Substring(scheme.Length + 1).Trim();
            return (string) null;
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = getToken(Context.Request);
            bool removeToken = false;

            try
            {
                if (token != null)
                {
                    removeToken = true;
                    Context.Items.Add(IdentityServerAuthenticationDefaults.TokenItemsKey, token);

                    var url = $"http://{Options.Authority}/connect/introspect";
                    var client = new HttpClient(Options.Hander, false);
                    var request = new HttpRequestMessage(HttpMethod.Post, url);
                    var form = new Dictionary<string, string>
                    {
                        {"token", token}
                    };
                    request.Content = new FormUrlEncodedContent(form);
                    request.Headers.Authorization = new BasicAuthenticationHeaderValue("api1", "secret");
                    var response = await client.SendAsync(request).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsObjectAsync<dynamic>();
                        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(), ""));
                    }
                    else
                    {
                        return AuthenticateResult.Fail("token is not active");
                    }
                }

                return AuthenticateResult.NoResult();
            }
            finally
            {
                if (removeToken)
                {
                    Context.Items.Remove(IdentityServerAuthenticationDefaults.TokenItemsKey);
                }
            }
        }
    }
}