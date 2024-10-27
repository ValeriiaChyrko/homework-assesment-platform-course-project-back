using Microsoft.EntityFrameworkCore.Storage;

namespace HomeAssignment.Database.Contexts.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void DetachEntitiesInChangeTracker();
    IDbContextTransaction BeginTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    void CommitTransaction(IDbContextTransaction transaction);
    void RollbackTransaction(IDbContextTransaction transaction);
}