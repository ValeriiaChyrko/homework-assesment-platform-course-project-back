using FluentValidation.TestHelper;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.validators;
using HomeAssignment.DTOs.SharedDTOs;

namespace HomeAssignment.DTOs.Tests.ValidatorsTests;

[TestFixture]
public class RequestAssignmentDtoValidatorTests
{
    private RequestAssignmentDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new RequestAssignmentDtoValidator();
    }

    [Test]
    public void Should_Have_Error_When_OwnerId_Is_Empty()
    {
        var dto = new RequestAssignmentDto
        {
            OwnerId = Guid.Empty,
            Title = "string",
            RepositoryName = "string",
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.OwnerId)
            .WithErrorMessage("OwnerId cannot be an empty GUID.");
    }
    [Test]
    public void Should_Not_Have_Error_When_OwnerId_Is_Valid()
    {
        var dto = new RequestAssignmentDto
        {
            OwnerId = Guid.NewGuid(),
            Title = "string",
            RepositoryName = "string",
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.OwnerId);
    }
    [Test]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var dto = new RequestAssignmentDto
        {
            OwnerId = Guid.NewGuid(),
            Title = string.Empty,
            RepositoryName = "string"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title cannot be empty.");
    }
    [Test]
    public void Should_Have_Error_When_Title_Exceeds_Max_Length()
    {
        var dto = new RequestAssignmentDto
        {
            OwnerId = Guid.NewGuid(),
            Title = new string('a', 65) ,
            RepositoryName = "string"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Full name cannot exceed 64 characters.");
    }
    [Test]
    public void Should_Have_Error_When_Deadline_Is_In_Past()
    {
        var dto = new RequestAssignmentDto
        {
            OwnerId = Guid.NewGuid(),
            Deadline = DateTime.UtcNow.AddDays(-1),
            Title = "string",
            RepositoryName = "string",
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Deadline)
            .WithErrorMessage("Date of the deadline must be after the current date.");
    }
    [Test]
    public void Should_Not_Have_Error_When_Deadline_Is_Valid()
    {
        var dto = new RequestAssignmentDto
        {
            OwnerId = Guid.NewGuid(),
            Deadline = DateTime.UtcNow.AddDays(5),
            Title = "string",
            RepositoryName = "string"
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Deadline);
    }
    [Test]
    public void Should_Have_Error_When_MaxScore_Is_Zero()
    {
        var dto = new RequestAssignmentDto
        {
            OwnerId = Guid.NewGuid(),
            Title = "string",
            RepositoryName = "string",
            Deadline = DateTime.UtcNow.AddDays(1),
            MaxScore = 0
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.MaxScore)
            .WithErrorMessage("The value of max score must be greater than zero.");
    }
    [Test]
    public void Should_Not_Have_Error_When_MaxScore_Is_Valid()
    {
        var dto = new RequestAssignmentDto
        {
            OwnerId = Guid.NewGuid(),
            Title = "string",
            RepositoryName = "string",
            Deadline = DateTime.UtcNow.AddDays(5),
            MaxScore = 10
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.MaxScore);
    }
    [Test]
    public void Should_Have_Error_When_MaxAttemptsAmount_Is_One()
    {
        var dto = new RequestAssignmentDto
        {
            OwnerId = Guid.NewGuid(),
            Title = "string",
            RepositoryName = "string",
            Deadline = DateTime.UtcNow.AddDays(1),
            MaxAttemptsAmount = 1
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.MaxAttemptsAmount)
            .WithErrorMessage("The value of max attempts amount must be greater than one.");
    }
    [Test]
    public void Should_Validate_CompilationSection_Using_ScoreSectionDtoValidator()
    {
        // Arrange
        var compilationSection = new ScoreSectionDto
        {
            IsEnabled = true,
            MaxScore = -5, // Invalid
            MinScore = -1  // Invalid
        };

        var dto = new RequestAssignmentDto
        {
            OwnerId = Guid.NewGuid(),
            Title = "string",
            RepositoryName = "string",
            Deadline = DateTime.UtcNow.AddDays(5),
            MaxScore = 10,
            MaxAttemptsAmount = 2,
            CompilationSection = compilationSection
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor("CompilationSection.MaxScore")
            .WithErrorMessage("The value of max score must be greater than zero.");
        result.ShouldHaveValidationErrorFor("CompilationSection.MinScore")
            .WithErrorMessage("The value of min score must be greater than zero.");
    }
}
