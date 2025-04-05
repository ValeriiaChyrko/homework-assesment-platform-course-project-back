using FluentValidation;
using HomeAssignment.Domain.Abstractions.Enums;

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
        
        RuleFor(dto => dto.Position)
            .NotNull().WithMessage("The value of max score cannot be empty.")
            .GreaterThan(0).WithMessage("The value of max score must be greater than one.")
            .When(dto => dto.Position != null);

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .When(x => x.BranchName != null);
    }
}