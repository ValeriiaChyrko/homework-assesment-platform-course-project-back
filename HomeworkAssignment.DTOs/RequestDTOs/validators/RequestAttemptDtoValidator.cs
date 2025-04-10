using FluentValidation;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestAttemptDtoValidator : AbstractValidator<RequestAttemptDto>
{
    public RequestAttemptDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(id => id != Guid.Empty).WithMessage("UserId cannot be an empty GUID.");

        RuleFor(x => x.AssignmentId)
            .NotEmpty().WithMessage("AssignmentId is required.")
            .Must(id => id != Guid.Empty).WithMessage("AssignmentId cannot be an empty GUID.");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .When(x => x.BranchName != null);
    }
}