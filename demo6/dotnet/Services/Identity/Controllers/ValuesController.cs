using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Pivotal.Discovery.Client;

namespace Identity.Controllers
{
    [Route("/values")]
    public class ValuesController : Controller
    {
        private readonly DiscoveryHttpClientHandler _handler;
        public ValuesController(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }
        [HttpGet]
        public IActionResult Get()
        {
            var client = new HttpClient(_handler, false);
            var url = $"http://user/search?name=leo&password=123456";
            var result = client.GetAsync(url).Result;
            if (result.IsSuccessStatusCode)
            {
                return Ok("leo");
            }
            return NotFound();
        }
    }
}
