using System.Text;
using System.Text.Json;

namespace MossadAPI.MiddleWares;

public class auth
{

    private readonly RequestDelegate _next;

    public auth(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        Console.WriteLine($"got request to server: {request.Method}{request.Path}" + $"from IP:{request.HttpContext.Connection.RemoteIpAddress}");
        await _next(context);
    }
}
