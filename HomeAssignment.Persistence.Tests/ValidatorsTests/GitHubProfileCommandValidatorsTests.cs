using FluentValidation.TestHelper;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.Persistence.Commands.GitHubProfiles;
using HomeAssignment.Persistence.Commands.GitHubProfiles.Validators;

namespace HomeAssignment.Persistence.Tests.ValidatorsTests;

[TestFixture]
public class GitHubProfileCommandValidatorsTests
{
    [Test]
    public void CreateGitHubProfileCommandValidator_Should_Have_Error_When_GitHubProfileDto_Is_Null()
    {
        // Arrange
        var validator = new CreateGitHubProfileCommandValidator();
        var command = new CreateGitHubProfileCommand(null!);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GitHubProfileDto)
            .WithErrorMessage("The GitHub profile object must be passed to the method.");
    }

    [Test]
    public void DeleteGitHubProfileCommandValidator_Should_Have_Error_When_Id_Is_Empty()
    {
        // Arrange
        var validator = new DeleteGitHubProfileCommandValidator();
        var command = new DeleteGitHubProfileCommand(Guid.Empty);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("The GitHub profile Id cannot be an empty GUID.");
    }

    [Test]
    public void DeleteGitHubProfileCommandValidator_Should_Not_Have_Error_When_Id_Is_Valid()
    {
        // Arrange
        var validator = new DeleteGitHubProfileCommandValidator();
        var command = new DeleteGitHubProfileCommand(Guid.NewGuid());

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void UpdateGitHubProfileCommandValidator_Should_Have_Error_When_GitHubProfileDto_Is_Null()
    {
        // Arrange
        var validator = new UpdateGitHubProfileCommandValidator();
        var command = new UpdateGitHubProfileCommand(null!);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GitHubProfileDto)
            .WithErrorMessage("The GitHub profile object must be passed to the method.");
    }

    [Test]
    public void CreateGitHubProfileCommandValidator_Should_Not_Have_Error_When_GitHubProfileDto_Is_Valid()
    {
        // Arrange
        var validDto = new GitHubProfileDto
        {
            Id = Guid.NewGuid(),
            GithubUsername = "testuser",
            GithubProfileUrl = "https://github.com/testuser"
        };
        var validator = new CreateGitHubProfileCommandValidator();
        var command = new CreateGitHubProfileCommand(validDto);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.GitHubProfileDto);
    }

    [Test]
    public void UpdateGitHubProfileCommandValidator_Should_Not_Have_Error_When_GitHubProfileDto_Is_Valid()
    {
        // Arrange
        var validDto = new GitHubProfileDto
        {
            Id = Guid.NewGuid(),
            GithubUsername = "testuser",
            GithubProfileUrl = "https://github.com/testuser"
        };
        var validator = new UpdateGitHubProfileCommandValidator();
        var command = new UpdateGitHubProfileCommand(validDto);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.GitHubProfileDto);
    }
}