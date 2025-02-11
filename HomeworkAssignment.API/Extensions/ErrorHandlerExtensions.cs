using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace HomeworkAssignment.Extensions;

public static class ErrorHandlerExtensions
{
    public static void UseErrorHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;

                var logger = context.RequestServices.GetService<ILogger<Program>>();
                logger?.LogError(exception, "An unhandled exception has occurred.");

                var response = new
                {
                    context.Response.StatusCode,
                    Message = "An unexpected error occurred. Please try again later.",
                    Detailed = exception?.Message 
                };

                var jsonResponse = JsonSerializer.Serialize(response);
                
                await context.Response.WriteAsync(jsonResponse);
            });
        });
    }
}