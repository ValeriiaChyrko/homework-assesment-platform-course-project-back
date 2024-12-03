using FluentValidation.TestHelper;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.DTOs.SharedDTOs.validators;

namespace HomeAssignment.DTOs.Tests.ValidatorsTests;

[TestFixture]
public class UserDtoValidatorTests
{
    private UserDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UserDtoValidator();
    }

    [Test]
    public void Should_Have_Error_When_FullName_Is_Empty()
    {
        var model = new UserDto
        {
            FullName = "",
            Email = "string",
            PasswordHash = "string",
            RoleType = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.FullName)
            .WithErrorMessage("Full name cannot be empty.");
    }
    [Test]
    public void Should_Have_Error_When_FullName_Exceeds_Max_Length()
    {
        var model = new UserDto
        {
            FullName = new string('A',
                129),
            Email = "string",
            PasswordHash = "string",
            RoleType = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.FullName)
            .WithErrorMessage("Full name cannot exceed 128 characters.");
    }
    [Test]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new UserDto
        {
            PasswordHash = "",
            FullName = "string",
            Email = "string",
            RoleType = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.PasswordHash)
            .WithErrorMessage("Password cannot be empty.");
    }
    [Test]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var model = new UserDto
        {
            PasswordHash = "Pass1!",
            FullName = "string",
            Email = "string",
            RoleType = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.PasswordHash)
            .WithErrorMessage("Password must be at least 8 characters long.");
    }
    [Test]
    public void Should_Have_Error_When_Password_Missing_Uppercase()
    {
        var model = new UserDto
        {
            PasswordHash = "password1!",
            FullName = "string",
            Email = "string",
            RoleType = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.PasswordHash)
            .WithErrorMessage("Password must contain at least one uppercase letter.");
    }
    [Test]
    public void Should_Have_Error_When_Password_Missing_Lowercase()
    {
        var model = new UserDto
        {
            PasswordHash = "PASSWORD1!",
            FullName = "string",
            Email = "string",
            RoleType = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.PasswordHash)
            .WithErrorMessage("Password must contain at least one lowercase letter.");
    }
    [Test]
    public void Should_Have_Error_When_Password_Missing_Number()
    {
        var model = new UserDto
        {
            PasswordHash = "Password!",
            FullName = "string",
            Email = "string",
            RoleType = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.PasswordHash)
            .WithErrorMessage("Password must contain at least one number.");
    }
    [Test]
    public void Should_Have_Error_When_Password_Missing_Special_Character()
    {
        var model = new UserDto
        {
            PasswordHash = "Password1",
            FullName = "string",
            Email = "string",
            RoleType = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.PasswordHash)
            .WithErrorMessage("Password must contain at least one special character.");
    }
    [Test]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new UserDto
        {
            Email = "invalidemail",
            FullName = "string",
            PasswordHash = "string",
            RoleType = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.Email)
            .WithErrorMessage("Invalid email format.");
    }
    [Test]
    public void Should_Not_Have_Any_Errors_For_Valid_Model()
    {
        var model = new UserDto
        {
            FullName = "Valid User",
            PasswordHash = "Password1!",
            Email = "valid@example.com",
            RoleType = "student"
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}