using System.Runtime.CompilerServices;
using HomeAssignment.Persistence.Abstractions.Exceptions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Abstractions;

public abstract class BaseService<T>(ILogger<T> logger, IDatabaseTransactionManager transactionManager)
{
    private readonly ILogger<T> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IDatabaseTransactionManager _transactionManager =
        transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));

    protected async Task<TResponse> ExecuteTransactionAsync<TResponse>(
        Func<Task<TResponse>> operation,
        [CallerMemberName] string? operationName = null,
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _transactionManager.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        using (_logger.BeginScope("Transaction: {OperationName}", operationName))
        {
            try
            {
                var result = await operation().ConfigureAwait(false);

                await Task.WhenAll(
                    _transactionManager.CommitAsync(cancellationToken),
                    LogCompletion(operationName)
                ).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                if (_transactionManager.HasActiveTransaction)
                    await _transactionManager.RollbackAsync(cancellationToken).ConfigureAwait(false);

                if (_logger.IsEnabled(LogLevel.Error))
                    _logger.LogError(ex, "Error during {OperationName}: {Message}", operationName, ex.Message);

                throw new ServiceOperationException($"Error during {operationName}. See inner exception for details.",
                    ex);
            }
        }
    }

    protected Task ExecuteTransactionAsync(
        Func<Task> operation,
        [CallerMemberName] string? operationName = null,
        CancellationToken cancellationToken = default)
    {
        return ExecuteTransactionAsync(async () =>
        {
            await operation().ConfigureAwait(false);
            return true;
        }, operationName, cancellationToken);
    }

    protected async Task<TResponse> ExecuteWithExceptionHandlingAsync<TResponse>(
        Func<Task<TResponse>> operation,
        [CallerMemberName] string? operationName = null)
    {
        using (_logger.BeginScope("Operation: {OperationName}", operationName))
        {
            try
            {
                return await operation().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                    _logger.LogError(ex, "Error during {OperationName}: {Message}", operationName, ex.Message);

                throw new ServiceOperationException($"Error during {operationName}. See inner exception for details.",
                    ex);
            }
        }
    }

    private Task LogCompletion(string? operationName)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Operation {OperationName} completed successfully.", operationName);

        return Task.CompletedTask;
    }
}