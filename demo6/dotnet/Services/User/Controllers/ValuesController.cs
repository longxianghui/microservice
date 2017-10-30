using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;
using Pivotal.Discovery.Client;
using User.Models;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace User.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
        private const string IdentityApplicationName = "identity";
        private readonly DiscoveryHttpClientHandler _handler;

        public ValuesController(IDiscoveryClient client)
        {
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
            //因为是做API的授权，clientid和clientSecret可以默认
            var url = $"http://{IdentityApplicationName}/connect/token";
            var client = new HttpClient(_handler, false);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var form = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"scope", "api1 offline_access"},
                {"username", input.Name},
                {"password", input.Password}
            };
            request.Content = new FormUrlEncodedContent(form);
            request.Headers.Authorization = new BasicAuthenticationHeaderValue("ro.client", "secret");
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsObjectAsync<dynamic>();
                return Ok(new LoginResponse()
                {
                    AccessToken= result.access_token,
                    RefreshToken = result.refresh_token,
                    ExpireIn = result.expires_in
                });
            }
            return NotFound();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var account = Account.GetAll().FirstOrDefault(x => x.Id == id);
            if (account != null)
            {
                return Ok(account);
            }
            else
            {
                return NotFound();
            }
        }
    }
}