using System.Net;
using System.Text.Json;
using HomeAssignment.Persistence.Abstractions.Exceptions;
using HomeAssignment.Persistence.Common.Exceptions;
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
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature == null) return;

                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.ContentType = "application/json";

                context.Response.StatusCode = contextFeature.Error switch
                {
                    RequestValidationException => (int)HttpStatusCode.BadRequest,
                    OperationCanceledException => (int)HttpStatusCode.ServiceUnavailable,
                    DatabaseErrorException => (int)HttpStatusCode.InternalServerError,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var errorResponse = new
                {
                    statusCode = context.Response.StatusCode,
                    message = contextFeature.Error.Message,
                    innerException = contextFeature.Error.InnerException?.Message,
                    clientException = GetErrorBody(contextFeature.Error)
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            });
        });
    }

    private static Dictionary<string, List<string>>? GetErrorBody(Exception error)
    {
        return error switch
        {
            RequestValidationException validationException => validationException.GetErrors(),
            DatabaseErrorException databaseErrorException => databaseErrorException.GetErrors(),
            _ => null
        };
    }
}