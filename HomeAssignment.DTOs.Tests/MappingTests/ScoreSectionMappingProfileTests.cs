using AutoMapper;
using FluentAssertions;
using HomeAssignment.Domain.Abstractions;
using HomeAssignment.DTOs.MappingProfiles;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.Tests.MappingTests;

public class ScoreSectionMappingProfileTests
{
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ScoreSectionMappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Test]
    public void ScoreSection_To_ScoreSectionDto_Should_Map_Correctly()
    {
        // Arrange
        var scoreSection = new ScoreSection(true, 100, 10);

        // Act
        var result = _mapper.Map<ScoreSectionDto>(scoreSection);

        // Assert
        result.IsEnabled.Should().Be(scoreSection.IsEnabled);
        result.MaxScore.Should().Be(scoreSection.MaxScore);
        result.MinScore.Should().Be(scoreSection.MinScore);
    }

    [Test]
    public void ScoreSectionDto_To_ScoreSection_Should_Map_Correctly()
    {
        // Arrange
        var scoreSectionDto = new ScoreSectionDto
        {
            IsEnabled = false,
            MaxScore = 80,
            MinScore = 20
        };

        // Act
        var result = _mapper.Map<ScoreSection>(scoreSectionDto);

        // Assert
        result.IsEnabled.Should().Be(scoreSectionDto.IsEnabled);
        result.MaxScore.Should().Be(scoreSectionDto.MaxScore);
        result.MinScore.Should().Be(scoreSectionDto.MinScore);
    }
}