using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestAssignmentDtoValidator : AbstractValidator<RequestAssignmentDto>
{
    private const int MaxLengthTitlePropertyLength = 64;
    private const int MaxLengthRepositoryPropertyLength = 64;
    private const int MaxLengthRepositoryUrlPropertyLength = 256;
    private const int MaxLengthDescriptionPropertyLength = 15000;

    public RequestAssignmentDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthTitlePropertyLength} characters.");
        
        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .MaximumLength(MaxLengthDescriptionPropertyLength)
            .WithMessage($"Specialization cannot exceed {MaxLengthDescriptionPropertyLength} characters.")
            .When(x => x.Description != null);

        RuleFor(dto => dto.RepositoryName)
            .NotEmpty().WithMessage("Repository name cannot be empty.")
            .MaximumLength(MaxLengthRepositoryPropertyLength)
            .WithMessage($"Repository name cannot exceed {MaxLengthRepositoryPropertyLength} characters.")
            .When(x => x.RepositoryName != null);
        
        RuleFor(dto => dto.RepositoryOwner)
            .NotEmpty().WithMessage("Repository owner cannot be empty.")
            .MaximumLength(MaxLengthRepositoryPropertyLength)
            .WithMessage($"Repository owner cannot exceed {MaxLengthRepositoryPropertyLength} characters.")
            .When(x => x.RepositoryOwner != null);
        
        RuleFor(dto => dto.RepositoryUrl)
            .NotEmpty().WithMessage("Repository URL cannot be empty.")
            .MaximumLength(MaxLengthRepositoryUrlPropertyLength)
            .WithMessage($"Repository URL cannot exceed {MaxLengthRepositoryUrlPropertyLength} characters.")
            .When(x => x.RepositoryUrl != null);

        RuleFor(dto => dto.Deadline)
            .NotEmpty().WithMessage("Date of the deadline cannot be empty.")
            .Must(BeValidDeadlineDate).WithMessage("Date of the deadline must be after the current date.")
            .When(x => x.Deadline != null);

        RuleFor(dto => dto.MaxScore)
            .GreaterThan(-1).WithMessage("The value of max score must be greater than zero.");

        RuleFor(dto => dto.MaxAttemptsAmount)
            .GreaterThan(-1).WithMessage("The value of max attempts amount must be greater than one.");
        
        RuleFor(dto => dto.Position)
            .GreaterThan(-1).WithMessage("The value of max score must be greater than one.");
        
        RuleFor(dto => dto.AttemptCompilationMaxScore)
            .NotNull().WithMessage("The value of max score cannot be empty.")
            .GreaterThan(-1).WithMessage("The value of max score must be greater than zero.");

        RuleFor(dto => dto.AttemptCompilationMinScore)
            .NotNull().WithMessage("The value of min score cannot be empty.")
            .GreaterThan(-1).WithMessage("The value of min score must be positive number.");
        
        RuleFor(dto => dto.AttemptTestsMaxScore)
            .GreaterThan(-1).WithMessage("The value of max score must be greater than zero.");

        RuleFor(dto => dto.AttemptTestsMinScore)
            .GreaterThan(-1).WithMessage("The value of min score must be positive number.");
        
        RuleFor(dto => dto.AttemptQualityMaxScore)
            .GreaterThan(-1).WithMessage("The value of max score must be greater than zero.");

        RuleFor(dto => dto.AttemptQualityMinScore)
            .GreaterThan(-1).WithMessage("The value of min score must be positive number.");
    }

    private static bool BeValidDeadlineDate(DateTime? dateTime)
    {
        return dateTime != null && dateTime > DateTime.UtcNow;
    }
}