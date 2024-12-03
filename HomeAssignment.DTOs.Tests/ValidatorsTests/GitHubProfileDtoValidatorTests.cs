using FluentValidation.TestHelper;
using HomeAssignment.DTOs.SharedDTOs;
using HomeAssignment.DTOs.SharedDTOs.validators;

namespace HomeAssignment.DTOs.Tests.ValidatorsTests;

[TestFixture]
public class GitHubProfileDtoValidatorTests
{
    private GitHubProfileDtoValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new GitHubProfileDtoValidator();
    }

    [Test]
    public void Should_Have_Error_When_GithubUsername_Is_Null_Or_Empty()
    {
        var model = new GitHubProfileDto
        {
            GithubUsername = null!,
            GithubProfileUrl = null!
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.GithubUsername)
            .WithErrorMessage("Github username cannot be empty.");
    }
    [Test]
    public void Should_Have_Error_When_GithubUsername_Exceeds_MaxLength()
    {
        var model = new GitHubProfileDto
        {
            GithubUsername = new string('a',
                65),
            GithubProfileUrl = null!
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.GithubUsername)
            .WithErrorMessage("Github username  cannot exceed 64 characters.");
    }
    [Test]
    public void Should_Have_Error_When_GithubProfileUrl_Is_Null_Or_Empty()
    {
        var model = new GitHubProfileDto
        {
            GithubProfileUrl = null!,
            GithubUsername = null!
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.GithubProfileUrl)
            .WithErrorMessage("Github profile url cannot be empty.");
    }
    [Test]
    public void Should_Have_Error_When_GithubProfileUrl_Exceeds_MaxLength()
    {
        var model = new GitHubProfileDto
        {
            GithubProfileUrl = new string('b',
                129),
            GithubUsername = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.GithubProfileUrl)
            .WithErrorMessage("Github profile url cannot exceed 128 characters.");
    }
    [Test]
    public void Should_Have_Error_When_GithubPictureUrl_Exceeds_MaxLength()
    {
        var model = new GitHubProfileDto
        {
            GithubPictureUrl = new string('c',
                129),
            GithubUsername = "string",
            GithubProfileUrl = "string"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(dto => dto.GithubPictureUrl)
            .WithErrorMessage("Github picture url cannot exceed 128 characters.");
    }
    [Test]
    public void Should_Not_Have_Any_Errors_When_Model_Is_Valid()
    {
        var model = new GitHubProfileDto
        {
            GithubUsername = "ValidUser",
            GithubProfileUrl = "https://github.com/ValidUser",
            GithubPictureUrl = "https://github.com/images/ValidUser.png"
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}