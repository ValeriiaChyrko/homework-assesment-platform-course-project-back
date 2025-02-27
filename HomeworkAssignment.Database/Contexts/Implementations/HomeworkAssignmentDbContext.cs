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
    public DbSet<AttachmentEntity> AttachmentEntities { get; set; } = null!;
    public DbSet<AttemptProgressEntity> AttemptProgressEntities { get; set; } = null!;
    public DbSet<CategoryEntity> CategoryEntities { get; set; } = null!;
    public DbSet<ChapterEntity> ChapterEntities { get; set; } = null!;
    public DbSet<CourseEntity> CourseEntities { get; set; } = null!;
    public DbSet<EnrollmentEntity> EnrollmentEntities { get; set; } = null!;
    public DbSet<MuxDataEntity> MuxDataEntities { get; set; } = null!;
    public DbSet<UserEntity> UserEntities { get; set; } = null!;
    public DbSet<UserProgressEntity> UserProgressEntities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(HomeworkAssignmentDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}