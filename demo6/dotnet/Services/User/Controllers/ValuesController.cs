using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;
using Pivotal.Discovery.Client;
using User.Models;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace User.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
        private const string IdentityApplicationName = "identity";
        private readonly DiscoveryHttpClientHandler _handler;
        private readonly IConfiguration _configuration;

        public ValuesController(IDiscoveryClient client, IConfiguration configuration)
        {
            _configuration = configuration;
            _handler = new DiscoveryHttpClientHandler(client);
        }
        [HttpGet("search")]
        public IActionResult Get(string name, string password)
        {
            var account = Account.GetAll().FirstOrDefault(x => x.Name == name && x.Password == password);
            if (account != null)
            {
                return Ok(account);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest input)
        {
            var discoveryClient = new DiscoveryClient($"http://{IdentityApplicationName}", _handler)
            {
                Policy = new DiscoveryPolicy { RequireHttps = false }
            };
            var disco = await discoveryClient.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var clientId = _configuration.GetSection("IdentityServer:ClientId").Value;
            if (string.IsNullOrEmpty(clientId)) throw new Exception("clientId is not value.");

            var clientSecrets = _configuration.GetSection("IdentityServer:ClientSecrets").Value;
            if (string.IsNullOrEmpty(clientSecrets)) throw new Exception("clientSecrets is not value.");

            var tokenClient = new TokenClient(disco.TokenEndpoint, clientId, clientSecrets, _handler);
            var response = await tokenClient.RequestResourceOwnerPasswordAsync(input.Name, input.Password, "api1");
            //var response = await tokenClient.RequestResourceOwnerPasswordAsync(input.Name, input.Password, "api1 offline_access");
            if (response.IsError) throw new Exception(response.Error);

            return Ok(new LoginResponse()
            {
                AccessToken = response.AccessToken,
                ExpireIn = response.ExpiresIn,
                RefreshToken = response.RefreshToken
            });
        }
    }
}