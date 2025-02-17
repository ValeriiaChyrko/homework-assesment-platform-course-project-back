using HomeAssignment.Persistence.Abstractions.Errors;
using HomeAssignment.Persistence.Abstractions.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeAssignment.Persistence.Behaviors;

public class DatabaseErrorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<DatabaseErrorBehavior<TRequest, TResponse>> _logger;

    public DatabaseErrorBehavior(ILogger<DatabaseErrorBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (DbUpdateException ex)
        {
            var errors = new List<DataBaseError>();
            var innerMessage = ex.InnerException?.Message;

            foreach (var entry in ex.Entries)
            {
                var tableName = entry.Entity.GetType().Name;
                var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);
                var members = databaseValues?.Properties;

                if (members != null)
                {
                    var uniqueErrors = new HashSet<string?>();

                    foreach (var _ in members) uniqueErrors.Add(innerMessage);

                    errors.AddRange(uniqueErrors.Select(message => new DataBaseError
                        { Table = tableName, Message = message }));
                }
                else
                {
                    _logger.LogWarning(
                        "No database values found for entity of type {EntityType}. This may indicate a data integrity issue.",
                        tableName);

                    errors.Add(new DataBaseError
                    {
                        Table = tableName,
                        Message = innerMessage
                    });
                }
            }

            _logger.LogError(ex,
                "Database error occurred while processing request of type {RequestType}. Number of errors: {ErrorCount}.",
                typeof(TRequest).Name, errors.Count);

            var errorMessage = $"Database error for query: {typeof(TRequest).Name}";
            throw new DatabaseErrorException(errorMessage, errors, ex.InnerException);
        }
    }
}