using System.Text.Json;
using Search.Model.Exceptions;

namespace Search.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            if (ex is not SearchException)
            {
                logger.LogError(ex,
                    $"Unhandled exception occurred for {httpContext.Request.Method} {httpContext.Request.Path}");
            }
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        (int statusCode, string err) = exception switch
        {
            SearchException ex => (ex.StatusCode, ex.Message),
            _ => (500, "Внутренняя ошибка сервера")
        };
        context.Response.StatusCode = statusCode;
        var response = new
        {
            error = err,
            status = statusCode
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}