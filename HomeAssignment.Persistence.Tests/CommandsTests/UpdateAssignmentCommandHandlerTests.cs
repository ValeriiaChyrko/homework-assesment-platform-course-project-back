using AutoMapper;
using FluentAssertions;
using HomeAssignment.Database.Contexts.Abstractions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.Assignments;
using NSubstitute;

namespace HomeAssignment.Persistence.Tests.CommandsTests;

[TestFixture]
public class UpdateAssignmentCommandHandlerTests
{
    [SetUp]
    public void Setup()
    {
        _mockContext = Substitute.For<IHomeworkAssignmentDbContext>();

        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<AssignmentMappingProfile>();
                cfg.AddProfile<ScoreSectionMappingProfile>();
            }
        );
        _mapper = config.CreateMapper();

        _handler = new UpdateAssignmentCommandHandler(_mockContext, _mapper);
    }

    private IHomeworkAssignmentDbContext _mockContext;
    private IMapper _mapper;
    private UpdateAssignmentCommandHandler _handler;

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
        var act = () => new UpdateAssignmentCommandHandler(null!, _mapper);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'context')");
    }

    [Test]
    public void Should_Throw_ArgumentNullException_When_Mapper_Is_Null()
    {
        var act = () => new UpdateAssignmentCommandHandler(_mockContext, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'mapper')");
    }

    [Test]
    public async Task Should_Update_Assignment_And_Return_RespondDto()
    {
        // Arrange
        var assignmentDto = new RequestAssignmentDto
        {
            OwnerId = Guid.NewGuid(),
            Title = "New Assignment",
            Description = "Description",
            RepositoryName = "Repo",
            Deadline = DateTime.UtcNow.AddDays(7),
            MaxScore = 100,
            MaxAttemptsAmount = 5,
            CompilationSection = new ScoreSectionDto { MaxScore = 30, MinScore = 10 },
            TestsSection = new ScoreSectionDto { MaxScore = 50, MinScore = 20 },
            QualitySection = new ScoreSectionDto { MaxScore = 20, MinScore = 5 }
        };
        var command = new UpdateAssignmentCommand(Guid.NewGuid(), assignmentDto);

        var expectedEntity = _mapper.Map<AssignmentEntity>(
            Assignment.Create(
                assignmentDto.OwnerId,
                assignmentDto.Title,
                assignmentDto.Description,
                assignmentDto.RepositoryName,
                assignmentDto.Deadline,
                assignmentDto.MaxScore,
                assignmentDto.MaxAttemptsAmount,
                _mapper.Map<ScoreSection>(assignmentDto.CompilationSection),
                _mapper.Map<ScoreSection>(assignmentDto.TestsSection),
                _mapper.Map<ScoreSection>(assignmentDto.QualitySection)
            )
        );
        expectedEntity.Id = command.Id;

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockContext.AssignmentEntities.Received(1).Update(Arg.Is<AssignmentEntity>(a =>
            a.Id == expectedEntity.Id &&
            a.Title == expectedEntity.Title &&
            a.Description == expectedEntity.Description
        ));

        result.Should().NotBeNull();
        result.Id.Should().Be(command.Id);
        result.Title.Should().Be(assignmentDto.Title);
        result.Description.Should().Be(assignmentDto.Description);
    }
}