using System.Net;
using System.Text.Json;
using Greentube.Weather.Models;

namespace Greentube.Weather.CustomMiddlewares;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            var logger = httpContext.RequestServices.GetRequiredService<ILogger<GlobalExceptionHandler>>();
            logger.LogError(ex, "Global Exception Handler catched {message}", ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.Clear();
        httpContext.Response.ContentType = "application/json";

        string result;
        
        switch (exception)
        {
            case ArgumentException:
                result = JsonSerializer.Serialize(ServiceResponse<string>.Error(exception.Message));
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            
            default:
                result = JsonSerializer.Serialize(ServiceResponse<string>.Error(exception.Message));
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }
        
        
        
        await httpContext.Response.WriteAsync(result);

    }
}