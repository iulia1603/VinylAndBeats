namespace VinylAndBeats.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepție netratată la {Method} {Path}",
                context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Acces interzis."),
            _ => (StatusCodes.Status500InternalServerError, "A apărut o eroare internă.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(new { success = false, message });
    }
}