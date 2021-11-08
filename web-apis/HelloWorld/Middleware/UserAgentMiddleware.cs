using System;
using System.Threading.Tasks;
using HelloWorld.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace HelloWorld.Middleware
{
    public class UserAgentMiddleware
    {
        private readonly RequestDelegate _next;

        public UserAgentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers.TryGetValue("User-agent", out var header);
            Console.WriteLine($"Request made with: \n------------------\n{header}");
            await _next(context);
        }
    }
}