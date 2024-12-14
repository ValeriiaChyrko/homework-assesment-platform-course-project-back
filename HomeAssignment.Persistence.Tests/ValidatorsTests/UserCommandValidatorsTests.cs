using FluentValidation.TestHelper;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.Users;
using HomeAssignment.Persistence.Commands.Users.Validators;

namespace HomeAssignment.Persistence.Tests.ValidatorsTests;

[TestFixture]
public class UserCommandValidatorsTests
{
    [Test]
    public void CreateUserCommandValidator_Should_Have_Error_When_UserDto_Is_Null()
    {
        // Arrange
        var validator = new CreateUserCommandValidator();
        var command = new CreateUserCommand(null!);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserDto)
            .WithErrorMessage("The User profile object must be passed to the method.");
    }

    [Test]
    public void DeleteUserCommandValidator_Should_Have_Error_When_Id_Is_Empty()
    {
        // Arrange
        var validator = new DeleteUserCommandValidator();
        var command = new DeleteUserCommand(Guid.Empty);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("The User profile Id cannot be an empty GUID.");
    }

    [Test]
    public void DeleteUserCommandValidator_Should_Not_Have_Error_When_Id_Is_Valid()
    {
        // Arrange
        var validator = new DeleteUserCommandValidator();
        var command = new DeleteUserCommand(Guid.NewGuid());

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void UpdateUserCommandValidator_Should_Have_Error_When_UserDto_Is_Null()
    {
        // Arrange
        var validator = new UpdateUserCommandValidator();
        var command = new UpdateUserCommand(null!);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserDto)
            .WithErrorMessage("The User profile object must be passed to the method.");
    }

    [Test]
    public void CreateUserCommandValidator_Should_Not_Have_Error_When_UserDto_Is_Valid()
    {
        // Arrange
        var validDto = new UserDto
        {
            Id = Guid.NewGuid(),
            Email = "testuser@example.com",
            FullName = "testuser",
            PasswordHash = "wklekop3030",
            RoleType = "user"
        };
        var validator = new CreateUserCommandValidator();
        var command = new CreateUserCommand(validDto);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserDto);
    }

    [Test]
    public void UpdateUserCommandValidator_Should_Not_Have_Error_When_UserDto_Is_Valid()
    {
        // Arrange
        var validDto = new UserDto
        {
            Id = Guid.NewGuid(),
            Email = "testuser@example.com",
            FullName = "testuser",
            PasswordHash = "wklekop3030",
            RoleType = "user"
        };
        var validator = new UpdateUserCommandValidator();
        var command = new UpdateUserCommand(validDto);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserDto);
    }
}