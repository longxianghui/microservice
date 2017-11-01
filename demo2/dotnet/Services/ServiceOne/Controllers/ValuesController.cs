using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;

namespace ServiceOne.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly DiscoveryHttpClientHandler _handler;
        private const string ProductUrl = "http://product/api/values";

        public ValuesController(IDiscoveryClient client, ILoggerFactory logFactory)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }

        [HttpGet("product")]
        public async Task<string> GoServiceTwoAsync()
        {
            var client = new HttpClient(_handler);
            return await client.GetStringAsync(ProductUrl);
        }
    }
}
