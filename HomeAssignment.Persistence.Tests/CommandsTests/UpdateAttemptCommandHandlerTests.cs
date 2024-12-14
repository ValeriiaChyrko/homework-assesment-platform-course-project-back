using AutoMapper;
using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.Persistence.Commands.Attempts;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.CommandsTests;

[TestFixture]
public class UpdateAttemptCommandHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AttemptMappingProfile>(); });
        _mapper = config.CreateMapper();

        _handler = new UpdateAttemptCommandHandler(_mockContext, _mapper);
    }

    private IHomeworkAssignmentDbContext _mockContext;
    private IMapper _mapper;
    private UpdateAttemptCommandHandler _handler;

    [Test]
    public void Should_Throw_ArgumentNullException_When_Command_Is_Null()
    {
        // Act
        Func<Task> act = async () => await _handler.Handle(null!, CancellationToken.None);

        // Assert
        act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'command')");
    }

    [Test]
    public void Should_Throw_ArgumentNullException_When_Context_Is_Null()
    {
        var act = () => new UpdateAttemptCommandHandler(null!, _mapper);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }

    [Test]
    public void Should_Throw_ArgumentNullException_When_Mapper_Is_Null()
    {
        var act = () => new UpdateAttemptCommandHandler(_mockContext, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'mapper')");
    }

    [Test]
    public async Task Should_Update_Attempt_And_Return_RespondDto()
    {
        // Arrange
        var attemptDto = new RequestAttemptDto
        {
            StudentId = Guid.NewGuid(),
            AssignmentId = Guid.NewGuid(),
            BranchName = "main",
            AttemptNumber = 1,
            CompilationScore = 80,
            TestsScore = 90,
            QualityScore = 85
        };
        var command = new UpdateAttemptCommand(Guid.NewGuid(), attemptDto);

        var expectedEntity = _mapper.Map<AttemptEntity>(
            Attempt.Create(
                attemptDto.StudentId,
                attemptDto.AssignmentId,
                attemptDto.BranchName,
                attemptDto.AttemptNumber,
                attemptDto.CompilationScore,
                attemptDto.TestsScore,
                attemptDto.QualityScore
            )
        );
        expectedEntity.Id = command.Id;

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockContext.AttemptEntities.Received(1).Update(Arg.Is<AttemptEntity>(a =>
            a.Id == expectedEntity.Id &&
            a.StudentId == expectedEntity.StudentId &&
            a.AssignmentId == expectedEntity.AssignmentId &&
            a.BranchName == expectedEntity.BranchName &&
            a.AttemptNumber == expectedEntity.AttemptNumber &&
            a.CompilationScore == expectedEntity.CompilationScore &&
            a.TestsScore == expectedEntity.TestsScore &&
            a.QualityScore == expectedEntity.QualityScore
        ));

        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.StudentId.Should().Be(attemptDto.StudentId);
        result.AssignmentId.Should().Be(attemptDto.AssignmentId);
        result.BranchName.Should().Be(attemptDto.BranchName);
    }
}