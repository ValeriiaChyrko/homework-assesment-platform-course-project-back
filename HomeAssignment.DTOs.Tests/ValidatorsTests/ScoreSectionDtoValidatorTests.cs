using FluentValidation.TestHelper;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.DTOs.SharedDTOs.validators;

namespace HomeAssignment.DTOs.Tests.ValidatorsTests;

[TestFixture]
public class ScoreSectionDtoValidatorTests
{
    private ScoreSectionDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new ScoreSectionDtoValidator();
    }

    [Test]
    public void Should_Have_Error_When_MaxScore_Is_Empty()
    {
        var model = new ScoreSectionDto { MaxScore = 0 };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.MaxScore)
            .WithErrorMessage("The value of max score must be greater than zero.");
    }
    [Test]
    public void Should_Have_Error_When_MaxScore_Is_Not_Greater_Than_Zero()
    {
        var model = new ScoreSectionDto { MaxScore = -1 };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.MaxScore)
            .WithErrorMessage("The value of max score must be greater than zero.");
    }
    [Test]
    public void Should_Have_Error_When_MinScore_Is_Empty()
    {
        var model = new ScoreSectionDto { MinScore = -1 };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.MinScore)
            .WithErrorMessage("The value of min score must be positive number.");
    }
    [Test]
    public void Should_Have_Error_When_MinScore_Is_Not_Greater_Than_Zero()
    {
        var model = new ScoreSectionDto { MinScore = -1 };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.MinScore)
            .WithErrorMessage("The value of min score must be positive number.");
    }
    [Test]
    public void Should_Not_Have_Any_Errors_When_Model_Is_Valid()
    {
        var model = new ScoreSectionDto
        {
            MaxScore = 10,
            MinScore = 5
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}