using System.Collections.Concurrent;

namespace BeersCheersVasis.API.Internal;

public sealed class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, (int Count, DateTime Window)> _clients = new();
    private const int MaxRequests = 5;
    private static readonly TimeSpan WindowSize = TimeSpan.FromMinutes(1);

    public RateLimitMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // Only rate-limit comment creation
        if (context.Request.Path.StartsWithSegments("/Comment/Create") && context.Request.Method == "POST")
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var now = DateTime.UtcNow;

            var entry = _clients.GetOrAdd(ip, _ => (0, now));
            if (now - entry.Window > WindowSize)
                entry = (0, now);

            if (entry.Count >= MaxRequests)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Too many comments. Please wait a minute.");
                return;
            }

            _clients[ip] = (entry.Count + 1, entry.Window);
        }

        await _next(context);
    }
}
