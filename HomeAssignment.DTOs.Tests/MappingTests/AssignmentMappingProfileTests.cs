using AutoMapper;
using FluentAssertions;
using HomeAssignment.Database.Entities;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.DTOs.RespondDTOs;

namespace HomeAssignment.DTOs.Tests.MappingTests;

[TestFixture]
public class AssignmentMappingProfileTests
{
    [SetUp]
    public void SetUp()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AssignmentMappingProfile>());
        _mapper = config.CreateMapper();
    }

    private IMapper _mapper;

    [Test]
    public void Should_Map_AssignmentEntity_To_RespondAssignmentDto()
    {
        // Arrange
        var assignmentEntity = new AssignmentEntity
        {
            AttemptCompilationSectionEnable = true,
            AttemptCompilationMaxScore = 100,
            AttemptCompilationMinScore = 50,
            AttemptTestsSectionEnable = true,
            AttemptTestsMaxScore = 80,
            AttemptTestsMinScore = 40,
            AttemptQualitySectionEnable = true,
            AttemptQualityMaxScore = 90,
            AttemptQualityMinScore = 45,
            Title = "Example text",
            RepositoryName = "Example repository"
        };

        // Act
        var result = _mapper.Map<RespondAssignmentDto>(assignmentEntity);

        // Assert
        result.Should().NotBeNull();
        result.CompilationSection.Should().NotBeNull();
        result.CompilationSection?.IsEnabled.Should().BeTrue();
        result.CompilationSection?.MaxScore.Should().Be(100);
        result.CompilationSection?.MinScore.Should().Be(50);

        result.TestsSection.Should().NotBeNull();
        result.TestsSection?.IsEnabled.Should().BeTrue();
        result.TestsSection?.MaxScore.Should().Be(80);
        result.TestsSection?.MinScore.Should().Be(40);

        result.QualitySection.Should().NotBeNull();
        result.QualitySection?.IsEnabled.Should().BeTrue();
        result.QualitySection?.MaxScore.Should().Be(90);
        result.QualitySection?.MinScore.Should().Be(45);

        result.Title.Should().Be("Example text");
        result.RepositoryName.Should().Be("Example repository");
    }

    [Test]
    public void Should_Map_AssignmentEntity_To_Assignment()
    {
        // Arrange
        var assignmentEntity = new AssignmentEntity
        {
            AttemptCompilationSectionEnable = true,
            AttemptCompilationMaxScore = 100,
            AttemptCompilationMinScore = 50,
            AttemptTestsSectionEnable = true,
            AttemptTestsMaxScore = 80,
            AttemptTestsMinScore = 40,
            AttemptQualitySectionEnable = true,
            AttemptQualityMaxScore = 90,
            AttemptQualityMinScore = 45,
            Title = "Example text",
            RepositoryName = "Example repository"
        };

        // Act
        var result = _mapper.Map<Assignment>(assignmentEntity);

        // Assert
        result.Should().NotBeNull();
        result.CompilationSection.Should().NotBeNull();
        result.CompilationSection.IsEnabled.Should().BeTrue();
        result.CompilationSection.MaxScore.Should().Be(100);
        result.CompilationSection.MinScore.Should().Be(50);

        result.TestsSection.Should().NotBeNull();
        result.TestsSection.IsEnabled.Should().BeTrue();
        result.TestsSection.MaxScore.Should().Be(80);
        result.TestsSection.MinScore.Should().Be(40);

        result.QualitySection.Should().NotBeNull();
        result.QualitySection.IsEnabled.Should().BeTrue();
        result.QualitySection.MaxScore.Should().Be(90);
        result.QualitySection.MinScore.Should().Be(45);
    }

    [Test]
    public void Should_Map_Assignment_To_RespondAssignmentDto()
    {
        // Arrange
        var assignment = new Assignment
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Example text",
            "Example description",
            "Example repository",
            DateTime.Now,
            270,
            10,
            new ScoreSection(true, 100, 50),
            new ScoreSection(true, 80, 40),
            new ScoreSection(true, 90, 45)
        );

        // Act
        var result = _mapper.Map<RespondAssignmentDto>(assignment);

        // Assert
        result.Should().NotBeNull();
        result.CompilationSection.Should().NotBeNull();
        result.CompilationSection!.IsEnabled.Should().BeTrue();
        result.CompilationSection.MaxScore.Should().Be(100);
        result.CompilationSection.MinScore.Should().Be(50);

        result.TestsSection.Should().NotBeNull();
        result.TestsSection!.IsEnabled.Should().BeTrue();
        result.TestsSection.MaxScore.Should().Be(80);
        result.TestsSection.MinScore.Should().Be(40);

        result.QualitySection.Should().NotBeNull();
        result.QualitySection!.IsEnabled.Should().BeTrue();
        result.QualitySection.MaxScore.Should().Be(90);
        result.QualitySection.MinScore.Should().Be(45);

        result.Title.Should().Be("Example text");
        result.RepositoryName.Should().Be("Example repository");
        result.Description.Should().Be("Example description");

        result.MaxScore.Should().Be(270);
        result.MaxAttemptsAmount.Should().Be(10);
    }
}