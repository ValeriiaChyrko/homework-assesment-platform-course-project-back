using FluentValidation;

namespace HomeAssignment.DTOs.SharedDTOs.validators;

public class ScoreSectionDtoValidator : AbstractValidator<ScoreSectionDto>
{
    public ScoreSectionDtoValidator()
    {
        RuleFor(dto => dto.MaxScore)
            .NotNull().WithMessage("The value of max score cannot be empty.")
            .GreaterThan(0).WithMessage("The value of max score must be greater than zero.");

        RuleFor(dto => dto.MinScore)
            .NotNull().WithMessage("The value of min score cannot be empty.")
            .GreaterThan(-1).WithMessage("The value of min score must be positive number.");
    }
}