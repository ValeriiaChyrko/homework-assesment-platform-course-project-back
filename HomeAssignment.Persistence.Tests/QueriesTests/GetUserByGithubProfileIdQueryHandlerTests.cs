using AutoMapper;
using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.Persistence.Queries.Users;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.QueriesTests;

[TestFixture]
public class GetUserByGithubProfileIdQueryHandlerTests
{
    private IHomeworkAssignmentDbContext _mockContext;
    private IMapper _mapper;
    private GetUserByGithubProfileIdQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _handler = new GetUserByGithubProfileIdQueryHandler(_mockContext, _mapper);
    }

    [Test]
    public void Constructor_Should_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        Action act = () => new GetUserByGithubProfileIdQueryHandler(null!, _mapper);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }
    [Test]
    public void Constructor_Should_Throw_ArgumentNullException_When_Mapper_Is_Null()
    {
        Action act = () => new GetUserByGithubProfileIdQueryHandler(_mockContext, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'mapper')");
    }
}