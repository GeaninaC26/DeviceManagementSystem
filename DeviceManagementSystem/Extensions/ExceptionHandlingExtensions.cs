using Microsoft.AspNetCore.Diagnostics;

namespace DeviceManagementSystem.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionFeature?.Error;

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = exception switch
                {
                    ArgumentException => StatusCodes.Status400BadRequest,
                    InvalidOperationException => StatusCodes.Status409Conflict,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError,
                };

                var message = exception switch
                {
                    null => "Unexpected server error.",
                    ArgumentException => exception.Message,
                    InvalidOperationException => exception.Message,
                    KeyNotFoundException => exception.Message,
                    UnauthorizedAccessException => exception.Message,
                    _ => "Unexpected server error.",
                };

                await context.Response.WriteAsJsonAsync(new { message });
            });
        });

        return app;
    }
}
