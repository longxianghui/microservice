using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace ConfigClient1.Controllers
{
    [Route("/")]
    public class ValuesController : Controller
    {
        private readonly IConfigurationRoot _config;
        private readonly IOptionsSnapshot<Demo> _configDemo;

        public ValuesController(IConfigurationRoot config, IOptionsSnapshot<Demo> configDemo)
        {
            _config = config;
            _configDemo = configDemo;
        }

        // GET api/values
        [HttpGet]
        public Demo Get()
        {
            //两种方式获取配置文件的数据
            //var demo = new Demo
            //{
            //    Name = _config["name"],
            //    Age = int.Parse(_config["age"]),
            //    Env = _config["env"]
            //};
            var demo = _configDemo.Value;
            return demo;
        }

        [HttpGet("/reload")]
        public string Reload()
        {
            _config?.Reload();
            return "reload";
        }
    }
}