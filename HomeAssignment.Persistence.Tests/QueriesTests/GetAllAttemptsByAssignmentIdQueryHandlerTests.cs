using AutoMapper;
using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.Persistence.Queries.Attempts;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.QueriesTests;

[TestFixture]
public class GetAllAttemptsByAssignmentIdQueryHandlerTests
{
    private IHomeworkAssignmentDbContext _mockContext;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AttemptMappingProfile>(); });
        _mapper = config.CreateMapper();
    }

    [Test]
    public void Constructor_Should_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        Action act = () => new GetAllAttemptsByAssignmentIdQueryHandler(null!, _mapper);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }
    [Test]
    public void Constructor_Should_Throw_ArgumentNullException_When_Mapper_Is_Null()
    {
        Action act = () => new GetAllAttemptsByAssignmentIdQueryHandler(_mockContext, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'mapper')");
    }
}