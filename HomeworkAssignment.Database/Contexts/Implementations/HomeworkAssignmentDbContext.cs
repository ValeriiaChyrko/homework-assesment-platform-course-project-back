using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HomeAssignment.Database.Contexts.Implementations;

public class HomeworkAssignmentDbContext : DbContext, IHomeworkAssignmentDbContext
{
    public HomeworkAssignmentDbContext(DbContextOptions<HomeworkAssignmentDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(HomeworkAssignmentDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }


    public void DetachEntitiesInChangeTracker()
    {
        foreach (var entry in ChangeTracker.Entries()) entry.State = EntityState.Detached;
    }

    public IDbContextTransaction BeginTransaction()
    {
        return Database.BeginTransaction();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    public void CommitTransaction(IDbContextTransaction transaction)
    {
        transaction.Commit();
    }

    public void RollbackTransaction(IDbContextTransaction transaction)
    {
        transaction.Rollback();
    }

    public DbSet<AssignmentEntity> AssignmentEntities { get; set; } = null!;
    public DbSet<AttemptEntity> AttemptEntities { get; set; } = null!;
    public DbSet<GitHubProfilesEntity> GitHubProfilesEntities { get; set; } = null!;
    public DbSet<UserEntity> UserEntities { get; set; } = null!;
}