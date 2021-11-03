using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HelloWorld.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class HelloWorldController : ControllerBase
    {
        
        private readonly ILogger<HelloWorldController> _logger;

        public HelloWorldController(ILogger<HelloWorldController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get(string name)
        {
            await Task.Delay(500);
            Request.Headers.TryGetValue("User-agent", out var header);
            return $"{name} was requested by {header}";
        }
        
    }
}