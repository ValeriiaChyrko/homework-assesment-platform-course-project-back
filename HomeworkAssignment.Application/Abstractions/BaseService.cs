using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.Persistence.Common.Exceptions;
using HomeworkAssignment.Application.Abstractions.Contracts;

namespace HomeworkAssignment.Application.Abstractions;

public abstract class BaseService
{
    private readonly ILogger _logger;
    private readonly IDatabaseTransactionManager _transactionManager;

    protected BaseService(ILogger logger, IDatabaseTransactionManager transactionManager)
    {
        _logger = logger;
        _transactionManager = transactionManager;
    }

    protected async Task<T> ExecuteWithTransactionAsync<T>(
        Func<Task<T>> operation,
        string operationName,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            var result = await operation();
            await _transactionManager.CommitAsync(transaction, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackAsync(transaction, cancellationToken);
            _logger.Log($"Error during {operationName}: {ex.Message}");
            throw new ServiceOperationException($"Error during {operationName}.", ex);
        }
    }

    protected async Task ExecuteWithTransactionAsync(
        Func<Task> operation,
        string operationName,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _transactionManager.BeginTransactionAsync();
        try
        {
            await operation();
            await _transactionManager.CommitAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackAsync(transaction, cancellationToken);
            _logger.Log($"Error during {operationName}: {ex.Message}");
            throw new ServiceOperationException($"Error during {operationName}.", ex);
        }
    }

    protected async Task<T> ExecuteWithExceptionHandlingAsync<T>(
        Func<Task<T>> operation,
        string operationName)
    {
        try
        {
            return await operation();
        }
        catch (Exception ex)
        {
            _logger.Log($"Error during {operationName}: {ex.Message}");
            throw new ServiceOperationException($"Error during {operationName}.", ex);
        }
    }
}