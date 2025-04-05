using HomeAssignment.Database.Contexts.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace HomeworkAssignment.Application.Abstractions;

public class DatabaseTransactionManager(IHomeworkAssignmentDbContext context) : IDatabaseTransactionManager
{
    private readonly IHomeworkAssignmentDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private IDbContextTransaction? _transaction;

    public bool HasActiveTransaction => _transaction != null;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public IDbContextTransaction? GetCurrentTransaction()
    {
        return _transaction;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null) throw new InvalidOperationException("Transaction is already active.");

        _transaction = await _context.BeginTransactionAsync(cancellationToken);
        return _transaction;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) throw new InvalidOperationException("No active transaction to commit.");

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _context.SaveChangesAsync(cancellationToken);
            _context.DetachEntitiesInChangeTracker();
            await _transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await RollbackAsync(cancellationToken);
            throw new InvalidOperationException("Error committing transaction", ex);
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null) throw new InvalidOperationException("No active transaction to rollback.");

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public void Dispose()
    {
        DisposeTransactionAsync().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeTransactionAsync();
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}