using FluentAssertions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeAssignment.Persistence.Abstractions.Exceptions;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Application.Abstractions.Contracts;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;

namespace HomeworkAssignment.Application.Tests;

[TestFixture]
public class BaseServiceTests
{
    private ILogger _loggerMock = null!;
    private IDatabaseTransactionManager _transactionManagerMock = null!;
    private TestService _service = null!;

    [SetUp]
    public void Setup()
    {
        _loggerMock = Substitute.For<ILogger>();
        _transactionManagerMock = Substitute.For<IDatabaseTransactionManager>();
        _service = new TestService(_loggerMock, _transactionManagerMock);
    }

    [Test]
    public async Task ExecuteWithTransactionAsync_ShouldCommitTransaction_WhenOperationSucceeds()
    {
        // Arrange
        var transactionMock = Substitute.For<IDbContextTransaction>();
        _transactionManagerMock.BeginTransactionAsync().Returns(transactionMock);

        async Task<int> Func() => await Task.FromResult(42);

        // Act
        var result = await _service.ExecuteWithTransactionAsync((Func<Task<int>>)Func, "TestOperation");

        // Assert
        result.Should().Be(42);
        await _transactionManagerMock.Received(1).CommitAsync(transactionMock, Arg.Any<CancellationToken>());
        await _transactionManagerMock.DidNotReceive().RollbackAsync(transactionMock, Arg.Any<CancellationToken>());
    }
    [Test]
    public async Task ExecuteWithTransactionAsync_ShouldRollbackTransactionAndLogError_WhenOperationFails()
    {
        // Arrange
        var transactionMock = Substitute.For<IDbContextTransaction>();
        _transactionManagerMock.BeginTransactionAsync().Returns(transactionMock);

        Func<Task> operation = () => throw new InvalidOperationException("Test exception");

        // Act
        var act = async () => await _service.ExecuteWithTransactionAsync(operation, "TestOperation");

        // Assert
        await act.Should().ThrowAsync<ServiceOperationException>()
            .WithMessage("Error during TestOperation.");

        await _transactionManagerMock.Received(1).RollbackAsync(transactionMock, Arg.Any<CancellationToken>());
        _loggerMock.Received(1).Log(Arg.Is<string>(msg => msg.Contains("Error during TestOperation: Test exception")));
    }
    [Test]
    public async Task ExecuteWithExceptionHandlingAsync_ShouldReturnResult_WhenOperationSucceeds()
    {
        // Act
        var result = await _service.ExecuteWithExceptionHandlingAsync((Func<Task<int>>)Func, "TestOperation");

        // Assert
        result.Should().Be(42);
        _loggerMock.DidNotReceive().Log(Arg.Any<string>());

        // Arrange
        async Task<int> Func() => await Task.FromResult(42);
    }
    [Test]
    public async Task ExecuteWithExceptionHandlingAsync_ShouldLogErrorAndThrow_WhenOperationFails()
    {
        // Arrange
        Func<Task<int>> operation = () => throw new InvalidOperationException("Test exception");

        // Act
        Func<Task> act = async () => await _service.ExecuteWithExceptionHandlingAsync(operation, "TestOperation");

        // Assert
        await act.Should().ThrowAsync<ServiceOperationException>()
            .WithMessage("Error during TestOperation.");

        _loggerMock.Received(1).Log(Arg.Is<string>(msg => msg.Contains("Error during TestOperation: Test exception")));
    }

    // Test class to test BaseService
    private class TestService : BaseService
    {
        public TestService(ILogger logger, IDatabaseTransactionManager transactionManager) 
            : base(logger, transactionManager)
        {
        }

        public new Task<T> ExecuteWithTransactionAsync<T>(Func<Task<T>> operation, string operationName, CancellationToken cancellationToken = default)
        {
            return base.ExecuteWithTransactionAsync(operation, operationName, cancellationToken);
        }

        public new Task ExecuteWithTransactionAsync(Func<Task> operation, string operationName, CancellationToken cancellationToken = default)
        {
            return base.ExecuteWithTransactionAsync(operation, operationName, cancellationToken);
        }

        public new Task<T> ExecuteWithExceptionHandlingAsync<T>(Func<Task<T>> operation, string operationName)
        {
            return base.ExecuteWithExceptionHandlingAsync(operation, operationName);
        }
    }
}
