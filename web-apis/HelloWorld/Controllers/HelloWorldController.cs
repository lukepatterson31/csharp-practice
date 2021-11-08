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
        public Task<string> Get(string name)
        {
            var agentResult = Request.Headers.TryGetValue("User-agent", out var header);
            var result = agentResult ? $"{name} was requested by {header}" : "No Agent found!!";
            return Task.FromResult(result);
        }
    }
}