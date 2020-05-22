using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace CleanTemplate.API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationId = "CorrelationId";
        private readonly RequestDelegate next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers.TryGetValue(CorrelationId, out var value);
            var correlationIdValue = value.FirstOrDefault() ?? Guid.NewGuid().ToString();
            // https://github.com/serilog/serilog/wiki/Enrichment
            using (LogContext.PushProperty(CorrelationId, correlationIdValue))
            {
                await next(context);
            }
        }
    }

    public static class CorrelationIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
