using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Order.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("order");
        }

      
    }
}
