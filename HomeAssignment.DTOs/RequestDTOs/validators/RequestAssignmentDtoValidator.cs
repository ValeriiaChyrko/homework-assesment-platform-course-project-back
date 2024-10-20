using FluentValidation;
using HomeAssignment.DTOs.SharedDTOs.validators;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestAssignmentDtoValidator : AbstractValidator<RequestAssignmentDto>
{
    private const int MaxLengthTitlePropertyLength = 64;
    private const int MaxLengthDescriptionPropertyLength = 512;

    public RequestAssignmentDtoValidator()
    {
        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.")
            .Must(id => id != Guid.Empty).WithMessage("OwnerId cannot be an empty GUID.");
        
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthTitlePropertyLength} characters.");
        
        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .MaximumLength(MaxLengthDescriptionPropertyLength)
            .WithMessage($"Specialization cannot exceed {MaxLengthDescriptionPropertyLength} characters.")
            .When(x => x.Description != null);

        RuleFor(dto => dto.Deadline)
            .NotEmpty().WithMessage("Date of the deadline cannot be empty.")
            .Must(BeValidDeadlineDate).WithMessage("Date of the deadline must be after the current date.");
        
        RuleFor(dto => dto.MaxScore)
            .NotEmpty().WithMessage("The value of max score cannot be empty.")
            .GreaterThan(0).WithMessage("The value of max score must be greater than zero.");
        
        RuleFor(dto => dto.MaxAttemptsAmount)
            .NotEmpty().WithMessage("The value of max attempts amount cannot be empty.")
            .GreaterThan(1).WithMessage("The value of max attempts amount must be greater than one.");

        RuleFor(p => p.CompilationSection)
            .SetValidator(new ScoreSectionDtoValidator()!)
            .When(p => p.CompilationSection != null);
        
        RuleFor(p => p.TestsSection)
            .SetValidator(new ScoreSectionDtoValidator()!)
            .When(p => p.TestsSection != null);
        
        RuleFor(p => p.QualitySection)
            .SetValidator(new ScoreSectionDtoValidator()!)
            .When(p => p.QualitySection != null);
    }

    private static bool BeValidDeadlineDate(DateTime dateTime)
    {
        return dateTime > DateTime.UtcNow;
    }
}