using System.Runtime.CompilerServices;
using HomeAssignment.Persistence.Abstractions.Exceptions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using Microsoft.Extensions.Logging;

namespace HomeworkAssignment.Application.Abstractions;

public abstract class BaseService<T>
{
    private readonly ILogger<T> _logger;
    private readonly IDatabaseTransactionManager _transactionManager;

    protected BaseService(ILogger<T> logger, IDatabaseTransactionManager transactionManager)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _transactionManager = transactionManager ?? throw new ArgumentNullException(nameof(transactionManager));
    }

    protected async Task<TResponse> ExecuteTransactionAsync<TResponse>(
        Func<Task<TResponse>> operation,
        [CallerMemberName] string? operationName = null,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await operation();
            await _transactionManager.CommitAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "Error during {OperationName}: {Message}", operationName, ex.Message);
            throw new ServiceOperationException($"Error during {operationName}. See inner exception for details.", ex);
        }
    }

    protected async Task ExecuteTransactionAsync(
        Func<Task> operation,
        [CallerMemberName] string? operationName = null,
        CancellationToken cancellationToken = default)
    {
        await ExecuteTransactionAsync(async () =>
        {
            await operation();
            return Task.CompletedTask;
        }, operationName, cancellationToken);
    }

    protected async Task<TResponse> ExecuteWithExceptionHandlingAsync<TResponse>(
        Func<Task<TResponse>> operation,
        [CallerMemberName] string? operationName = null)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during {OperationName}: {Message}", operationName, ex.Message);
            throw new ServiceOperationException($"Error during {operationName}. See inner exception for details.", ex);
        }
    }
}