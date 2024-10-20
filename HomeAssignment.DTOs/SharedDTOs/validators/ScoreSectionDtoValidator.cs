using FluentValidation;

namespace HomeAssignment.DTOs.SharedDTOs.validators;

public class ScoreSectionDtoValidator : AbstractValidator<ScoreSectionDto>
{
    public ScoreSectionDtoValidator()
    {
        RuleFor(dto => dto.MaxScore)
            .NotEmpty().WithMessage("The value of max score cannot be empty.")
            .GreaterThan(0).WithMessage("The value of max score must be greater than zero.");
        
        RuleFor(dto => dto.MinScore)
            .NotEmpty().WithMessage("The value of min score cannot be empty.")
            .GreaterThan(0).WithMessage("The value of min score must be greater than zero.");
    }
}