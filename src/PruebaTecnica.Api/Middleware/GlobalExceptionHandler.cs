using System.Text.Json;
using PruebaTecnica.Application.Common.Exceptions;
using PruebaTecnica.Application.Common.Models;

namespace PruebaTecnica.Api.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate next;
    private readonly ILogger<GlobalExceptionHandler> logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        this.next = next;
        this.logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception caught: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var env = context.RequestServices
                   .GetRequiredService<IWebHostEnvironment>();

        int statusCode;
        string message;

        if (exception is BaseException ex)
        {
            statusCode = ex.StatusCode;
            message = ex.Message;
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
            message = env.IsDevelopment()
                ? exception.Message
                : "Error interno del servidor.";
        }

        context.Response.StatusCode = statusCode;
        var response = ApiResponse<object>.Failure(message, statusCode);

        await context.Response.WriteAsJsonAsync(response);
    }
}
