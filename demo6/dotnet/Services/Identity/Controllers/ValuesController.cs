using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
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
        public IActionResult Login()
        {
            return Ok();
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
