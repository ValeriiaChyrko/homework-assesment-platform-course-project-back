using FluentValidation;

namespace HomeAssignment.DTOs.SharedDTOs.validators;

public class GitHubProfileDtoValidator : AbstractValidator<GitHubProfileDto>
{
    private const int MaxLengthUserNamePropertyLength = 64;
    private const int MaxLengthUrlPropertyLength = 128;

    public GitHubProfileDtoValidator()
    {
        RuleFor(dto => dto.GithubUsername)
            .NotNull().NotEmpty().WithMessage("Github username cannot be empty.")
            .MaximumLength(MaxLengthUserNamePropertyLength)
            .WithMessage($"Github username  cannot exceed {MaxLengthUserNamePropertyLength} characters.");

        RuleFor(dto => dto.GithubProfileUrl)
            .NotNull().NotEmpty().WithMessage("Github profile url cannot be empty.")
            .MaximumLength(MaxLengthUrlPropertyLength)
            .WithMessage($"Github profile url cannot exceed {MaxLengthUrlPropertyLength} characters.");

        RuleFor(dto => dto.GithubPictureUrl)
            .NotEmpty().WithMessage("Github picture url cannot be empty.")
            .MaximumLength(MaxLengthUrlPropertyLength)
            .WithMessage($"Github picture url cannot exceed {MaxLengthUrlPropertyLength} characters.")
            .When(dto => dto.GithubPictureUrl != null);
    }
}