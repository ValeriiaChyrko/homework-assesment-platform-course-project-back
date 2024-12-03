using AutoMapper;
using FluentAssertions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.Tests.MappingTests;

[TestFixture]
public class AttemptMappingProfileTests
{
    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<AttemptMappingProfile>(); });
        _mapper = config.CreateMapper();
    }

    private IMapper _mapper;

    [Test]
    public void Attempt_To_AttemptEntity_Should_Map_Correctly()
    {
        // Arrange
        var attempt = Attempt.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "feature/test",
            1,
            80,
            90,
            70
        );

        // Act
        var result = _mapper.Map<AttemptEntity>(attempt);

        // Assert
        result.Id.Should().BeEmpty();
        result.Student.Should().BeNull();
        result.Assignment.Should().BeNull();
        result.AttemptNumber.Should().Be(attempt.AttemptNumber);
        result.BranchName.Should().Be(attempt.BranchName);
        result.FinishedAt.Should().Be(attempt.FinishedAt);
        result.CompilationScore.Should().Be(attempt.CompilationScore);
        result.TestsScore.Should().Be(attempt.TestsScore);
        result.QualityScore.Should().Be(attempt.QualityScore);
        result.FinalScore.Should().Be(attempt.FinalScore);
    }

    [Test]
    public void AttemptEntity_To_Attempt_Should_Map_Correctly()
    {
        // Arrange
        var attemptEntity = new AttemptEntity
        {
            Id = Guid.NewGuid(),
            AttemptNumber = 2,
            BranchName = "feature/attempt2",
            FinishedAt = DateTime.UtcNow,
            CompilationScore = 70,
            TestsScore = 60,
            QualityScore = 50,
            FinalScore = 180
        };

        // Act
        var result = _mapper.Map<Attempt>(attemptEntity);

        // Assert
        result.Id.Should().Be(attemptEntity.Id);
        result.AttemptNumber.Should().Be(attemptEntity.AttemptNumber);
        result.BranchName.Should().Be(attemptEntity.BranchName);
        result.FinishedAt.Should().Be(attemptEntity.FinishedAt);
        result.CompilationScore.Should().Be(attemptEntity.CompilationScore);
        result.TestsScore.Should().Be(attemptEntity.TestsScore);
        result.QualityScore.Should().Be(attemptEntity.QualityScore);
        result.FinalScore.Should().Be(attemptEntity.FinalScore);
    }

    [Test]
    public void AttemptEntity_To_RespondAttemptDto_Should_Map_Correctly()
    {
        // Arrange
        var attemptEntity = new AttemptEntity
        {
            Id = Guid.NewGuid(),
            AttemptNumber = 3,
            BranchName = "feature/attempt3",
            FinishedAt = DateTime.UtcNow,
            CompilationScore = 85,
            TestsScore = 75,
            QualityScore = 65,
            FinalScore = 225
        };

        // Act
        var result = _mapper.Map<RespondAttemptDto>(attemptEntity);

        // Assert
        result.Id.Should().Be(attemptEntity.Id);
        result.AttemptNumber.Should().Be(attemptEntity.AttemptNumber);
        result.BranchName.Should().Be(attemptEntity.BranchName);
        result.FinishedAt.Should().Be(attemptEntity.FinishedAt);
        result.CompilationScore.Should().Be(attemptEntity.CompilationScore);
        result.TestsScore.Should().Be(attemptEntity.TestsScore);
        result.QualityScore.Should().Be(attemptEntity.QualityScore);
        result.FinalScore.Should().Be(attemptEntity.FinalScore);
    }

    [Test]
    public void RespondAttemptDto_To_AttemptEntity_Should_Map_Correctly()
    {
        // Arrange
        var dto = new RespondAttemptDto
        {
            Id = Guid.NewGuid(),
            AttemptNumber = 4,
            BranchName = "feature/attempt4",
            FinishedAt = DateTime.UtcNow,
            CompilationScore = 90,
            TestsScore = 85,
            QualityScore = 80,
            FinalScore = 255
        };

        // Act
        var result = _mapper.Map<AttemptEntity>(dto);

        // Assert
        result.Id.Should().Be(dto.Id);
        result.AttemptNumber.Should().Be(dto.AttemptNumber);
        result.BranchName.Should().Be(dto.BranchName);
        result.FinishedAt.Should().Be(dto.FinishedAt);
        result.CompilationScore.Should().Be(dto.CompilationScore);
        result.TestsScore.Should().Be(dto.TestsScore);
        result.QualityScore.Should().Be(dto.QualityScore);
        result.FinalScore.Should().Be(dto.FinalScore);
    }
}