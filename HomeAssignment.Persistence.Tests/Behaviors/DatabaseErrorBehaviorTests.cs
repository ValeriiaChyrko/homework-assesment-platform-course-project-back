using FluentAssertions;
using HomeAssignment.Persistence.Abstractions.Exceptions;
using HomeAssignment.Persistence.Behaviors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace HomeAssignment.Persistence.Tests.Behaviors;

[TestFixture]
public class DatabaseErrorBehaviorTests
{
    [SetUp]
    public void SetUp()
    {
        _next = Substitute.For<RequestHandlerDelegate<string>>();
        _cancellationToken = CancellationToken.None;
        _behavior = new DatabaseErrorBehavior<TestRequest, string?>();
    }

    private DatabaseErrorBehavior<TestRequest, string?> _behavior = null!;
    private RequestHandlerDelegate<string> _next = null!;
    private CancellationToken _cancellationToken;

    [Test]
    public async Task Handle_NoException_ReturnsResultFromNext()
    {
        // Arrange
        const string expectedResponse = "Success";
        _next().Returns(expectedResponse);

        var request = new TestRequest();

        // Act
        var result = await _behavior.Handle(request, _next!, _cancellationToken);

        // Assert
        result.Should().Be(expectedResponse);
    }

    [Test]
    public async Task Handle_DbUpdateException_CatchesAndThrowsDatabaseErrorException()
    {
        // Arrange
        var expectedExceptionMessage = "Test database exception";
        var dbUpdateException = new DbUpdateException(expectedExceptionMessage);
        _next().Throws(dbUpdateException);

        var request = new TestRequest();

        // Act
        Func<Task> act = async () => await _behavior.Handle(request, _next!, _cancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<DatabaseErrorException>();
        exception.Which.Message.Should().Contain($"Database error for query: {nameof(TestRequest)}");
    }

    [Test]
    public async Task Handle_DbUpdateException_WithInnerException_CatchesAndThrowsDatabaseErrorException()
    {
        // Arrange
        var innerExceptionMessage = "Inner exception message";
        var innerException = new Exception(innerExceptionMessage);
        var dbUpdateException = new DbUpdateException("Test database exception", innerException);
        _next().Throws(dbUpdateException);

        var request = new TestRequest();

        // Act
        Func<Task> act = async () => await _behavior.Handle(request, _next!, _cancellationToken);

        // Assert
        var exception = await act.Should().ThrowAsync<DatabaseErrorException>();
        exception.Which.InnerException.Should().Be(innerException);
    }

    [Test]
    public async Task Handle_NormalException_PropagatesTheException()
    {
        // Arrange
        var expectedException = new Exception("Test exception");
        _next().Throws(expectedException);

        var request = new TestRequest();

        // Act
        Func<Task> act = async () => await _behavior.Handle(request, _next!, _cancellationToken);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Test exception");
    }

    public class TestRequest : IRequest<string>
    {
    }

    public class TestEntity
    {
    }

    public class TestDbUpdateException : DbUpdateException
    {
        public TestDbUpdateException(string message) : base(message)
        {
        }

        public void AddEntry(EntityEntry entry)
        {
            (Entries as List<EntityEntry>)?.Add(entry);
        }
    }
}