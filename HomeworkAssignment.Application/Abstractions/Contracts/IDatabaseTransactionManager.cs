using Microsoft.EntityFrameworkCore.Storage;

namespace HomeworkAssignment.Application.Abstractions.Contracts;

public interface IDatabaseTransactionManager : IDisposable, IAsyncDisposable
{
    bool HasActiveTransaction { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    IDbContextTransaction? GetCurrentTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
}