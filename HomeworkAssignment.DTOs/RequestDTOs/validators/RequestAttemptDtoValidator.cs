using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestAttemptDtoValidator : AbstractValidator<RequestAttemptDto>
{
    public RequestAttemptDtoValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("StudentId is required.")
            .Must(id => id != Guid.Empty).WithMessage("StudentId cannot be an empty GUID.");

        RuleFor(x => x.AssignmentId)
            .NotEmpty().WithMessage("AssignmentId is required.")
            .Must(id => id != Guid.Empty).WithMessage("AssignmentId cannot be an empty GUID.");

        RuleFor(x => x.BranchName)
            .NotNull().NotEmpty().WithMessage("Branch name is required.");

        RuleFor(dto => dto.AttemptNumber)
            .NotEmpty().WithMessage("The value of attempt number cannot be empty.")
            .GreaterThan(1).WithMessage("The value of attempt number must be greater than one.");

        RuleFor(dto => dto.CompilationScore)
            .NotEmpty().WithMessage("The value of compilation score cannot be empty.")
            .GreaterThan(0).WithMessage("The value of compilation score must be greater than zero.");

        RuleFor(dto => dto.TestsScore)
            .NotEmpty().WithMessage("The value of tests score cannot be empty.")
            .GreaterThan(0).WithMessage("The value of tests score must be greater than zero.");

        RuleFor(dto => dto.QualityScore)
            .NotEmpty().WithMessage("The value of quality score cannot be empty.")
            .GreaterThan(0).WithMessage("The value of quality score must be greater than zero.");
    }
}