using FluentValidation;
using HomeAssignment.Domain.Abstractions.Enums;

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
        
        RuleFor(dto => dto.Position)
            .NotNull().WithMessage("The value of max score cannot be empty.")
            .GreaterThan(0).WithMessage("The value of max score must be greater than one.");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .When(x => x.BranchName != null);

        RuleFor(dto => dto.FinalScore)
            .NotNull().WithMessage("The value of final score cannot be empty.")
            .GreaterThan(-1).WithMessage("The value of final score must be greater than zero.");

        RuleFor(dto => dto.CompilationScore)
            .NotNull().WithMessage("The value of compilation score cannot be empty.")
            .GreaterThan(-1).WithMessage("The value of compilation score must be positive number.");

        RuleFor(dto => dto.TestsScore)
            .NotNull().WithMessage("The value of tests score cannot be empty.")
            .GreaterThan(-1).WithMessage("The value of tests score must be positive number.");

        RuleFor(dto => dto.QualityScore)
            .NotNull().WithMessage("The value of quality score cannot be empty.")
            .GreaterThan(-1).WithMessage("The value of quality score must be positive number.");
        
        RuleFor(dto => dto.ProgressStatus)
            .Must(BeAValidProgressStatus).WithMessage("SortBy must be a valid property name.");
    }

    private static bool BeAValidProgressStatus(string progressStatus)
    {
        return Enum.TryParse(typeof(ProgressStatuses), progressStatus, true, out _);
    }
}