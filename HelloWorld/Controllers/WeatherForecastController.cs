using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private int slaMs = 5000;
        
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
            Ping p1 = new Ping();
            List<string> listIPAdresses = new List<string> {"facebook.com", "amazon.com", "apple.com", "google.com"};
            
            
            Stopwatch timer = new Stopwatch();
            Console.WriteLine($"** Timer started");
            timer.Start();


            IEnumerable<WeatherForecast> result = null;

            using var selfCancellingCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(slaMs));

            using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, selfCancellingCancellationTokenSource.Token);
            
            Console.WriteLine("** Processing...");
            
            
            // await Task.Delay(2000);


            if (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                Console.WriteLine("** Starting request...");
                
                foreach (string ipAdress in listIPAdresses)
                {
                    if (timer.Elapsed.Milliseconds < 50)
                    {
                        var ping = p1.SendPingAsync( ipAdress, 5000);
                        var pingReply = ping.GetAwaiter().GetResult();
                        Console.WriteLine($"--Ping results after {timer.Elapsed.TotalMilliseconds} ms-- \nIP pinged: {pingReply.Address} \nStatus: {pingReply.Status} \nRoundtrip time: {pingReply.RoundtripTime}");
                    }

                    else
                    {
                        Console.WriteLine($"--Cancelled after {timer.Elapsed.TotalMilliseconds} ms--");
                        cancellationTokenSource.Cancel();
                    }
                    
                }
                
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
                Console.WriteLine("** Request Cancelled");    
            }
            
            Console.WriteLine($"** Finished in: {timer.Elapsed.TotalMilliseconds} ms");
            return await Task.FromResult(result);
        }
    }
}
