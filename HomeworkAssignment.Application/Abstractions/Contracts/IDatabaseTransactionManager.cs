using Microsoft.EntityFrameworkCore.Storage;

namespace HomeworkAssignment.Application.Abstractions.Contracts;

public interface IDatabaseTransactionManager : IDisposable, IAsyncDisposable
{
    bool HasActiveTransaction { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    IDbContextTransaction? GetCurrentTransaction();

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task CommitAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);

    Task RollbackAsync(IDbContextTransaction transaction, CancellationToken cancellationToken);
}