using HomeAssignment.Database.Contexts.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace HomeworkAssignment.Application.Implementations;

public class DatabaseTransactionManager : IDatabaseTransactionManager
{
    private readonly IHomeworkAssignmentDbContext _context;
    private IDbContextTransaction? _transaction;

    public DatabaseTransactionManager(IHomeworkAssignmentDbContext context)
    {
        _context = context;
    }

    public bool HasActiveTransaction => _transaction != null;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(cancellationToken);
        }

        return await _context.SaveChangesAsync(cancellationToken);
    }

    public IDbContextTransaction? GetCurrentTransaction()
    {
        return _transaction;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Transaction is already active.");
        }

        _transaction = await _context.BeginTransactionAsync();

        return _transaction;
    }

    public async Task CommitAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        if (_transaction != transaction)
        {
            throw new InvalidOperationException("Transaction is not current.");
        }

        try
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            _context.DetachEntitiesInChangeTracker();
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await RollbackAsync(transaction, cancellationToken);
            throw new Exception("Error committing transaction", ex);
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        if (_transaction != transaction)
        {
            throw new InvalidOperationException("Transaction is not current.");
        }

        try
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            await transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _transaction = null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
    }
}