using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestBranchDtoValidator : AbstractValidator<RequestBranchDto>
{
    public RequestBranchDtoValidator()
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
        
        RuleFor(x => x.Since)
            .Must((dto, since) => !dto.Until.HasValue || !since.HasValue || since <= dto.Until)
            .WithMessage("The 'Since' date cannot be later than the 'Until' date.");

        RuleFor(x => x.Since)
            .Must(since => !since.HasValue || since <= DateTime.UtcNow)
            .WithMessage("The 'Since' date cannot be in the future.");

        RuleFor(x => x.Until)
            .Must(until => !until.HasValue || until <= DateTime.UtcNow)
            .WithMessage("The 'Until' date cannot be in the future.");
    }
}