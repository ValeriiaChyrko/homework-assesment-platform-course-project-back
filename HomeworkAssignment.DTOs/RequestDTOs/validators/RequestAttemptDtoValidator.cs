using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestAttemptDtoValidator : AbstractValidator<AttemptDto>
{
    public RequestAttemptDtoValidator()
    {
        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.");
    }
}