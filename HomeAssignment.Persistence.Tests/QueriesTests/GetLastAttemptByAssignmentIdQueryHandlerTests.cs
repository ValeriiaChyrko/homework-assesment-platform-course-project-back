using AutoMapper;
using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.Persistence.Queries.Attempts;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.QueriesTests;

[TestFixture]
public class GetLastAttemptByAssignmentIdQueryHandlerTests
{
    private IHomeworkAssignmentDbContext _mockContext;
    private IMapper _mapper;
    private GetLastAttemptByAssignmentIdQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AttemptMappingProfile>(); });
        _mapper = config.CreateMapper();

        _handler = new GetLastAttemptByAssignmentIdQueryHandler(_mockContext, _mapper);
    }

    [Test]
    public void Constructor_Should_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        Action act = () => new GetLastAttemptByAssignmentIdQueryHandler(null!, _mapper);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }
    [Test]
    public void Constructor_Should_Not_Throw_When_Mapper_Is_Null()
    {
        Action act = () => new GetLastAttemptByAssignmentIdQueryHandler(_mockContext, null!);

        act.Should().NotThrow();
    }
    [Test]
    public void Handle_Should_Throw_Exception_When_Query_Is_Null()
    {
        // Act
        Func<Task> act = async () => await _handler.Handle(null!, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'query')");
    }
}