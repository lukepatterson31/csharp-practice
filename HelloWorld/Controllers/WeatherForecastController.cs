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
            Ping p1 = new Ping();
            Ping p2 = new Ping()
            List<Ping> pingRequests = new List<Ping> {,,,,,};
            List<string> listIpAdresses = new List<string> {"facebook.com", "amazon.com", "apple.com", "google.com"};
            
            
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
                
                foreach (string ip in listIpAdresses)
                {
                    p1 = new Ping();
                    if (timer.Elapsed.Milliseconds < 50)
                    {
                        Task<PingReply> sendPing = p1.SendAsync(ip, 1000); // use our cancellation token
                        PingReply sentPingReply = sendPing.GetAwaiter().GetResult(); // if we want to await async operations use await keyword
                        IPAddress pingedIpAddress = sentPingReply.Address;
                        IPStatus pingedIpStatus = sentPingReply.Status;
                        long pingedIpRoundtripTime = sentPingReply.RoundtripTime;

                    
                        _logger.LogInformation(
                            "--Ping results after {TimerMillisecondsElapsed} ms-- \n" +
                            "IP pinged: {PingedIpAddress} \n" +
                            "Status: {PingedIpStatus} \n" +
                            "Roundtrip time: {PingedIpRoundtripTime}"
                            ,timer.Elapsed.TotalMilliseconds ,pingedIpAddress, pingedIpStatus,pingedIpRoundtripTime);
                    }

                    else
                    {
                        _logger.LogError("--Cancelled after {TimerMillisecondsElapsed} ms--", timer.Elapsed.TotalMilliseconds);
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
                _logger.LogWarning("** Request Cancelled");    
            }
            
            _logger.LogInformation("** Finished in: {TimerMillisecondsElapsed} ms", timer.Elapsed.TotalMilliseconds);
            timer.Stop();
            return await Task.FromResult(result);
        }
    }
}
