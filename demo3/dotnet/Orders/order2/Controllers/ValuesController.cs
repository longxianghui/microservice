using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Order2.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return "order2";
        }
    }
}
