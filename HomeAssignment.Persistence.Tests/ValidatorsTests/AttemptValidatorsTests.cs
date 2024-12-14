using FluentValidation.TestHelper;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.Persistence.Commands.Attempts;
using HomeAssignment.Persistence.Commands.Attempts.Validators;

namespace HomeAssignment.Persistence.Tests.ValidatorsTests;

[TestFixture]
public class AttemptValidatorsTests
{
    [Test]
    public void CreateAttemptCommandValidator_Should_Have_Validation_Error_When_AttemptDto_Is_Null()
    {
        // Arrange
        var validator = new CreateAttemptCommandValidator();
        var command = new CreateAttemptCommand(null!);

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AttemptDto)
            .WithErrorMessage("The attempt object must be passed to the method.");
    }

    [Test]
    public void DeleteAttemptCommandValidator_Should_Have_Validation_Error_When_Id_Is_Empty()
    {
        // Arrange
        var validator = new DeleteAttemptCommandValidator();
        var command = new DeleteAttemptCommand(Guid.Empty);

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("The attempt Id cannot be an empty GUID.");
    }

    [Test]
    public void DeleteAttemptCommandValidator_Should_Not_Have_Validation_Error_When_Id_Is_Valid()
    {
        // Arrange
        var validator = new DeleteAttemptCommandValidator();
        var command = new DeleteAttemptCommand(Guid.NewGuid());

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void UpdateAttemptCommandValidator_Should_Have_Validation_Error_When_AttemptDto_Is_Null()
    {
        // Arrange
        var validator = new UpdateAttemptCommandValidator();
        var command = new UpdateAttemptCommand(Guid.NewGuid(), null!);

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AttemptDto)
            .WithErrorMessage("The attempt object must be passed to the method.");
    }

    [Test]
    public void UpdateAttemptCommandValidator_Should_Have_Validation_Error_When_Id_Is_Empty()
    {
        // Arrange
        var validator = new UpdateAttemptCommandValidator();
        var command = new UpdateAttemptCommand(Guid.Empty, new RequestAttemptDto());

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("The attempt Id cannot be an empty GUID.");
    }

    [Test]
    public void UpdateAttemptCommandValidator_Should_Not_Have_Validation_Error_When_All_Fields_Are_Valid()
    {
        // Arrange
        var validator = new UpdateAttemptCommandValidator();
        var command = new UpdateAttemptCommand(Guid.NewGuid(), new RequestAttemptDto
        {
            AttemptNumber = 1
        });

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.AttemptDto);
    }
}