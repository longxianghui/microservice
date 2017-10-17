using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Order1.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "order1";
        }
    }
}
