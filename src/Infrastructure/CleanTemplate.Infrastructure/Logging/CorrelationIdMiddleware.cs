using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace CleanTemplate.Infrastructure.Logging
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationId = "CorrelationId";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers.TryGetValue(CorrelationId, out var value);
            var correlationIdValue = value.FirstOrDefault() ?? Guid.NewGuid().ToString();
            // https://github.com/serilog/serilog/wiki/Enrichment
            using (LogContext.PushProperty(CorrelationId, correlationIdValue))
            {
                await _next(context);
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
