using FluentValidation.TestHelper;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.validators;

namespace HomeAssignment.DTOs.Tests.ValidatorsTests;

[TestFixture]
public class RequestAttemptDtoValidatorTests
{
    private RequestAttemptDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new RequestAttemptDtoValidator();
    }

    [Test]
    public void Should_Have_Error_When_StudentId_Is_Empty()
    {
        var model = new RequestAttemptDto { StudentId = Guid.Empty };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.StudentId)
            .WithErrorMessage("StudentId cannot be an empty GUID.");
    }
    [Test]
    public void Should_Have_Error_When_AssignmentId_Is_Empty()
    {
        var model = new RequestAttemptDto { AssignmentId = Guid.Empty };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.AssignmentId)
            .WithErrorMessage("AssignmentId cannot be an empty GUID.");
    }
    [Test]
    public void Should_Have_Error_When_BranchName_Is_Null_Or_Empty()
    {
        var model = new RequestAttemptDto { BranchName = null! };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.BranchName)
            .WithErrorMessage("Branch name is required.");
    }
    [Test]
    public void Should_Have_Error_When_AttemptNumber_Is_Invalid()
    {
        var model = new RequestAttemptDto { AttemptNumber = 0 };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.AttemptNumber)
            .WithErrorMessage("The value of attempt number must be greater than one.");
    }
    [Test]
    public void Should_Have_Error_When_CompilationScore_Is_Invalid()
    {
        var model = new RequestAttemptDto { CompilationScore = -1 };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.CompilationScore)
            .WithErrorMessage("The value of compilation score must be greater than zero.");
    }
    [Test]
    public void Should_Have_Error_When_TestsScore_Is_Invalid()
    {
        var model = new RequestAttemptDto { TestsScore = 0 };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.TestsScore)
            .WithErrorMessage("The value of tests score must be greater than zero.");
    }
    [Test]
    public void Should_Have_Error_When_QualityScore_Is_Invalid()
    {
        var model = new RequestAttemptDto { QualityScore = -5 };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.QualityScore)
            .WithErrorMessage("The value of quality score must be greater than zero.");
    }
    [Test]
    public void Should_Not_Have_Any_Errors_When_Model_Is_Valid()
    {
        var model = new RequestAttemptDto
        {
            StudentId = Guid.NewGuid(),
            AssignmentId = Guid.NewGuid(),
            BranchName = "main",
            AttemptNumber = 2,
            CompilationScore = 50,
            TestsScore = 90,
            QualityScore = 80
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}