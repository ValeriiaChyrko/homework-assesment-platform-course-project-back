using AutoMapper;
using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.Persistence.Commands.Attempts;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.CommandsTests;

[TestFixture]
public class CreateAttemptCommandHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();

        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AttemptMappingProfile>(); });
        _mapper = config.CreateMapper();

        _handler = new CreateAttemptCommandHandler(_mockContext, _mapper);
    }

    private IHomeworkAssignmentDbContext _mockContext;
    private IMapper _mapper;
    private CreateAttemptCommandHandler _handler;

    [Test]
    public void Should_Throw_ArgumentNullException_When_Command_Is_Null()
    {
        Func<Task> act = async () => await _handler.Handle(null!, CancellationToken.None);

        act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'command')");
    }

    [Test]
    public void Should_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        var act = () => new CreateAttemptCommandHandler(null!, _mapper);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }

    [Test]
    public void Should_Throw_ArgumentNullException_When_Mapper_Is_Null()
    {
        var act = () => new CreateAttemptCommandHandler(_mockContext, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'mapper')");
    }
}