using FluentAssertions;
using HomeAssignment.Persistence.Abstractions.Exceptions;
using HomeAssignment.Persistence.Behaviors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HomeAssignment.Persistence.Tests.Behaviors;

[TestFixture]
public class DatabaseErrorBehaviorDataSimulationTests
{
    [SetUp]
    public void SetUp()
    {
        _options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(_options);
        _behavior = new DatabaseErrorBehavior<TestRequest, string>();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public DbSet<TestEntity> TestEntities { get; } = null!;
    }

    private class TestEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    private class TestRequest : IRequest<string>
    {
    }

    private DbContextOptions<TestDbContext> _options = null!;
    private TestDbContext _context = null!;
    private DatabaseErrorBehavior<TestRequest, string> _behavior = null!;

    [Test]
    public async Task Handle_DatabaseError_ThrowsDatabaseErrorException()
    {
        // Arrange
        _context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test" });
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var requestDelegate = new RequestHandlerDelegate<string>(() =>
        {
            var entry = _context.Entry(new TestEntity { Id = 1, Name = "Test Updated" });
            entry.State = EntityState.Modified;

            throw new DbUpdateException("Test Exception", new Exception("Inner Exception"),
                new List<EntityEntry> { entry });
        });

        // Act
        Func<Task> act = async () =>
            await _behavior.Handle(new TestRequest(), requestDelegate, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<DatabaseErrorException>()
            .WithMessage("Database error for query: TestRequest");

        var errors = exception.Which.GetErrors();
        errors.Should().ContainKey("Database processing error");
    }

    [Test]
    public async Task Handle_DatabaseError_WithMultipleEntries_ThrowsDatabaseErrorException()
    {
        // Arrange: Add test entities to the context and save changes
        _context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "Test" },
            new TestEntity { Id = 2, Name = "Test2" }
        );
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Create a request delegate that simulates a database update exception
        var requestDelegate = new RequestHandlerDelegate<string>(() =>
        {
            var entry1 = _context.Entry(new TestEntity { Id = 1, Name = "Test Updated" });
            entry1.State = EntityState.Modified;

            var entry2 = _context.Entry(new TestEntity { Id = 2, Name = "Test2 Updated" });
            entry2.State = EntityState.Modified;

            // Throw a DbUpdateException to simulate a database error
            throw new DbUpdateException("Test Exception", new Exception("Inner Exception"),
                new List<EntityEntry> { entry1, entry2 });
        });

        // Act
        Func<Task> act = async () =>
            await _behavior.Handle(new TestRequest(), requestDelegate, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<DatabaseErrorException>()
            .WithMessage("Database error for query: TestRequest");
        var errors = exception.Which.GetErrors();
        errors.Should().ContainKey("Database processing error");
        errors["Database processing error"].Should().Contain("A database operation error occurred.");
    }

    [Test]
    public async Task Handle_SuccessfulRequest_ReturnsResponse()
    {
        _context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test" });
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var requestDelegate = new RequestHandlerDelegate<string>(() => Task.FromResult("Success"));

        var result = await _behavior.Handle(new TestRequest(), requestDelegate, CancellationToken.None);

        result.Should().Be("Success");
    }

    [Test]
    public async Task Handle_DatabaseError_WithNullMembers_ThrowsDatabaseErrorException()
    {
        // Arrange
        _context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test" });
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var requestDelegate = new RequestHandlerDelegate<string>(() =>
        {
            var entry = _context.Entry(new TestEntity { Id = 1, Name = "Test Updated" });
            entry.State = EntityState.Modified;

            throw new DbUpdateException("Test Exception", new Exception("Inner Exception"),
                new List<EntityEntry> { entry });
        });

        // Act
        Func<Task> act = async () =>
            await _behavior.Handle(new TestRequest(), requestDelegate, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<DatabaseErrorException>()
            .WithMessage("Database error for query: TestRequest");

        var errors = exception.Which.GetErrors();
        errors.Should().ContainKey("Database processing error");
    }

    [Test]
    public async Task Handle_DatabaseError_WithNoEntries_ThrowsDatabaseErrorException()
    {
        var requestDelegate = new RequestHandlerDelegate<string>(
            () => throw new DbUpdateException("Test Exception",
                new Exception("Inner Exception"), new List<EntityEntry>()));

        Func<Task> act = async () =>
            await _behavior.Handle(new TestRequest(), requestDelegate, CancellationToken.None);

        var exception = await act.Should().ThrowAsync<DatabaseErrorException>()
            .WithMessage("Database error for query: TestRequest");

        exception.Which.GetErrors().Should().BeEmpty();
    }
}