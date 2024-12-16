using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeworkAssignment.Application.Implementations;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace HomeworkAssignment.Application.Tests;

[TestFixture]
public class DatabaseTransactionManagerTests
{
    private IHomeworkAssignmentDbContext _mockContext;
    private DatabaseTransactionManager _transactionManager;
    private IDbContextTransaction _mockTransaction;

    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();
        _mockTransaction = Substitute.For<IDbContextTransaction>();
        _mockContext.BeginTransactionAsync().Returns(_mockTransaction);
        _transactionManager = new DatabaseTransactionManager(_mockContext);
    }

    [Test]
    public async Task BeginTransactionAsync_ShouldStartTransaction_WhenNoActiveTransaction()
    {
        var transaction = await _transactionManager.BeginTransactionAsync();

        transaction.Should().Be(_mockTransaction);
        _transactionManager.HasActiveTransaction.Should().BeTrue();
    }
    [Test]
    public void BeginTransactionAsync_ShouldThrow_WhenTransactionAlreadyActive()
    {
        var act = async () =>
        {
            await _transactionManager.BeginTransactionAsync();
            await _transactionManager.BeginTransactionAsync();
        };

        act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Transaction is already active.");
    }
    [Test]
    public void GetCurrentTransaction_ShouldReturnCurrentTransaction()
    {
        _transactionManager.GetCurrentTransaction().Should().BeNull();

        _transactionManager.BeginTransactionAsync().Wait();

        _transactionManager.GetCurrentTransaction().Should().Be(_mockTransaction);
    }
    [Test]
    public async Task CommitAsync_ShouldCommitTransaction_WhenTransactionIsCurrent()
    {
        await _transactionManager.BeginTransactionAsync();

        await _transactionManager.CommitAsync(_mockTransaction, CancellationToken.None);

        await _mockContext.Received(1).SaveChangesAsync(CancellationToken.None);
        await _mockTransaction.Received(1).CommitAsync(CancellationToken.None);
        _transactionManager.HasActiveTransaction.Should().BeFalse();
    }
    [Test]
    public void CommitAsync_ShouldThrow_WhenTransactionIsNull()
    {
        var act = async () => await _transactionManager.CommitAsync(null!, CancellationToken.None);

        act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'transaction')");
    }
    [Test]
    public async Task CommitAsync_ShouldRollbackAndThrow_WhenSaveChangesFails()
    {
        await _transactionManager.BeginTransactionAsync();
        _mockContext.SaveChangesAsync(Arg.Any<CancellationToken>()).Throws(new Exception("Save failed"));

        var act = async () => await _transactionManager.CommitAsync(_mockTransaction, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>().WithMessage("Error committing transaction");

        await _mockTransaction.Received(1).RollbackAsync(CancellationToken.None);
        _transactionManager.HasActiveTransaction.Should().BeFalse();
    }
    [Test]
    public async Task RollbackAsync_ShouldRollbackTransaction_WhenTransactionIsCurrent()
    {
        await _transactionManager.BeginTransactionAsync();

        await _transactionManager.RollbackAsync(_mockTransaction, CancellationToken.None);

        await _mockTransaction.Received(1).RollbackAsync(CancellationToken.None);
        _transactionManager.HasActiveTransaction.Should().BeFalse();
    }
    [Test]
    public void RollbackAsync_ShouldThrow_WhenTransactionIsNull()
    {
        Func<Task> act = async () => await _transactionManager.RollbackAsync(null!, CancellationToken.None);

        act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'transaction')");
    }
    [Test]
    public void Dispose_ShouldDisposeTransaction_WhenActive()
    {
        _transactionManager.BeginTransactionAsync().Wait();

        _transactionManager.Dispose();

        _mockTransaction.Received(1).Dispose();
        _transactionManager.HasActiveTransaction.Should().BeFalse();
    }
    [Test]
    public async Task DisposeAsync_ShouldDisposeTransaction_WhenActive()
    {
        await _transactionManager.BeginTransactionAsync();

        await _transactionManager.DisposeAsync();

        await _mockTransaction.Received(1).DisposeAsync();
        _transactionManager.HasActiveTransaction.Should().BeFalse();
    }
}