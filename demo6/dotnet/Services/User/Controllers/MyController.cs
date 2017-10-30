using Microsoft.AspNetCore.Mvc;
using Pivotal.Discovery.Client;

namespace User.Controllers
{
    [Route("/my")]
    public class MyController : Controller
    {
        private readonly IDiscoveryClient _discoveryClient;
        public MyController(IDiscoveryClient discoveryClient)
        {
            _discoveryClient = discoveryClient;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var sss = _discoveryClient.Description;
            return Ok();
        }
    }
}