using FluentValidation.TestHelper;
using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.Persistence.Commands.Assignments;
using HomeAssignment.Persistence.Commands.Assignments.Validators;

namespace HomeAssignment.Persistence.Tests.ValidatorsTests;

[TestFixture]
public class AssignmentValidatorsTests
{
    [Test]
    public void CreateAssignmentCommandValidator_Should_Have_Validation_Error_When_AssignmentDto_Is_Null()
    {
        // Arrange
        var validator = new CreateAssignmentCommandValidator();
        var command = new CreateAssignmentCommand(null!);

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AssignmentDto)
            .WithErrorMessage("The assignment object must be passed to the method.");
    }

    [Test]
    public void DeleteAssignmentCommandValidator_Should_Have_Validation_Error_When_Id_Is_Empty()
    {
        // Arrange
        var validator = new DeleteAssignmentCommandValidator();
        var command = new DeleteAssignmentCommand(Guid.Empty);

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("The assignment Id cannot be an empty GUID.");
    }

    [Test]
    public void DeleteAssignmentCommandValidator_Should_Have_Validation_Error_When_Id_Is_Not_Provided()
    {
        // Arrange
        var validator = new DeleteAssignmentCommandValidator();
        var command = new DeleteAssignmentCommand(Guid.NewGuid());

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void UpdateAssignmentCommandValidator_Should_Have_Validation_Error_When_AssignmentDto_Is_Null()
    {
        // Arrange
        var validator = new UpdateAssignmentCommandValidator();
        var command = new UpdateAssignmentCommand(Guid.NewGuid(), null!);

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.AssignmentDto)
            .WithErrorMessage("The assignment object must be passed to the method.");
    }

    [Test]
    public void UpdateAssignmentCommandValidator_Should_Have_Validation_Error_When_Id_Is_Empty()
    {
        // Arrange
        var validator = new UpdateAssignmentCommandValidator();
        var command = new UpdateAssignmentCommand(Guid.Empty, new RequestAssignmentDto
        {
            Title = "string",
            RepositoryName = "string"
        });

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("The assignment Id cannot be an empty GUID.");
    }

    [Test]
    public void UpdateAssignmentCommandValidator_Should_Not_Have_Validation_Errors_When_All_Fields_Are_Valid()
    {
        // Arrange
        var validator = new UpdateAssignmentCommandValidator();
        var command = new UpdateAssignmentCommand(Guid.NewGuid(), new RequestAssignmentDto
        {
            Title = "Valid Assignment",
            Description = "Valid Description",
            RepositoryName = "Valid Name"
        });

        // Act & Assert
        var result = validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Id);
        result.ShouldNotHaveValidationErrorFor(x => x.AssignmentDto);
    }
}