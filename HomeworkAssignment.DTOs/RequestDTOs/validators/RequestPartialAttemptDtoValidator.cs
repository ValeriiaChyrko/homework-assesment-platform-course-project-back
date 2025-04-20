using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestPartialAttemptDtoValidator : AbstractValidator<RequestPartialAttemptDto>
{
    public RequestPartialAttemptDtoValidator()
    {
        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .When(x => x.BranchName != null);
    }
}