using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestRepositoryWithBranchDtoValidator : AbstractValidator<RequestRepositoryWithBranchDto>
{
    public RequestRepositoryWithBranchDtoValidator()
    {
        RuleFor(x => x.RepoTitle)
            .NotEmpty()
            .WithMessage("Repository title cannot be null or whitespace.");

        RuleFor(x => x.OwnerGitHubUsername)
            .NotEmpty()
            .WithMessage("Owner GitHub username cannot be null or whitespace.");

        RuleFor(x => x.AuthorGitHubUsername)
            .NotEmpty()
            .WithMessage("Author GitHub username cannot be null or whitespace.");

        RuleFor(x => x.BranchTitle)
            .NotEmpty()
            .WithMessage("Branch title cannot be null or whitespace.");
    }
}