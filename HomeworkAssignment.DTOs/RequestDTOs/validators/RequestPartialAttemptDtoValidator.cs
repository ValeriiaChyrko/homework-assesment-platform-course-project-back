using FluentValidation;
using HomeAssignment.Domain.Abstractions.Enums;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestPartialAttemptDtoValidator : AbstractValidator<RequestPartialAttemptDto>
{
    public RequestPartialAttemptDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(id => id != Guid.Empty).WithMessage("UserId cannot be an empty GUID.");

        RuleFor(x => x.AssignmentId)
            .NotEmpty().WithMessage("AssignmentId is required.")
            .Must(id => id != Guid.Empty).WithMessage("AssignmentId cannot be an empty GUID.")
            .When(x => x.AssignmentId != null && x.AssignmentId != Guid.Empty);

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .When(x => x.BranchName != null);
    }
}