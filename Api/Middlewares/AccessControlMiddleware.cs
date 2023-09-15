using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shahrbin.Api.Middlewares
{

    public class AccessControlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _interval;
        private readonly int _limit;

        public AccessControlMiddleware(RequestDelegate next)
        {
            _next = next;
            _interval = 60;
            _limit = 45;
        }

        public async Task InvokeAsync(HttpContext httpContext, IMemoryCache memoryCache)
        {
            if (!httpContext.Request.Path.StartsWithSegments("/eventhub"))
            {
                var method = httpContext.Request.Method;
                if (method == "POST" || method == "PUT")
                {
                    var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    userId ??= "";
                    var ip = httpContext.Connection.RemoteIpAddress;
                    var key = $"{ip} {userId}";

                    if (memoryCache.TryGetValue(key, out var value))
                    {
                        if ((int)value < _limit)
                        {
                            memoryCache.Set(key, (int)value + 1, TimeSpan.FromSeconds(_interval));
                        }
                        else
                        {
                            httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                            await httpContext.Response.StartAsync();
                        }
                    }
                    else
                    {
                        memoryCache.Set(key, 1, TimeSpan.FromSeconds(_interval));
                    }

                }
            }
            await _next(httpContext);
        }
    }

    public static class AccessControlMiddlewareExtensions
    {
        public static IApplicationBuilder UseAccessControlMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccessControlMiddleware>();
        }
    }
}