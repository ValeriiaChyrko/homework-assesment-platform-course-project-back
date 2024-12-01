using FluentValidation;

namespace HomeAssignment.Persistence.Commands.GitHubProfiles.validators;

public class DeleteGitHubProfileCommandValidator : AbstractValidator<DeleteGitHubProfileCommand>
{
    public DeleteGitHubProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The GitHub profile Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("The GitHub profile Id cannot be an empty GUID.");
    }
}