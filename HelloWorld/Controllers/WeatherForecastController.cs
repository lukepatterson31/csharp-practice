using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HelloWorld.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private int slaMs = 3000;
        
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;


        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken cancellationToken)
        {
            Pingv2 p1 = new Pingv2();
            List<string> listIpAdresses = new List<string> {"https://www.facebook.com", "https://www.amazon.com", "https://www.apple.com", "https://www.google.com"};
            
            
            Stopwatch timer = new Stopwatch();
            _logger.LogInformation("** Timer started");
            timer.Start();


            IEnumerable<WeatherForecast> result = null;

            using var selfCancellingCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(slaMs));

            using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, selfCancellingCancellationTokenSource.Token);
            
            _logger.LogInformation("** Processing...");

            if (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                _logger.LogInformation("** Starting request...");
                
                var rng = new Random();
                result = Enumerable.Range(1, 5).Select(index => new WeatherForecast 
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55), 
                    Summary = Summaries[rng.Next(Summaries.Length)]

                }).ToArray();

                
            }
            else
            {
                _logger.LogWarning("** Request Cancelled");    
            }
            
            _logger.LogInformation("** Finished in: {TimerMillisecondsElapsed} ms", timer.Elapsed.TotalMilliseconds);
            timer.Stop();
            return await Task.FromResult(result);
        }
    }
}
