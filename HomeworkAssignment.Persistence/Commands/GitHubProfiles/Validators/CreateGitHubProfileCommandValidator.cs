using FluentValidation;
using HomeAssignment.DTOs.SharedDTOs.validators;

namespace HomeAssignment.Persistence.Commands.GitHubProfiles.Validators;

public class CreateGitHubProfileCommandValidator : AbstractValidator<CreateGitHubProfileCommand>
{
    public CreateGitHubProfileCommandValidator()
    {
        RuleFor(x => x.GitHubProfileDto)
            .NotNull().WithMessage("The GitHub profile object must be passed to the method.")
            .SetValidator(new GitHubProfileDtoValidator());
    }
}