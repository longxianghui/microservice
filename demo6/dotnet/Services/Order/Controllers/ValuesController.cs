using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Order.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet("admin")]
        [Authorize(Roles = "admin")]
        public IActionResult Get1()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var role = User.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
            return Ok(new { userId, role });
        }

        [HttpGet("normal")]
        [Authorize(Roles = "normal")]
        public IActionResult Get2()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            return Ok(new { role = "normal", userId = userId });
        }
        [HttpGet("any")]
        [Authorize]
        public IActionResult Get3()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            return Ok(new { role = "any", userId = userId });
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(new { role = "allowAnonymous" });
        }
    }
}
