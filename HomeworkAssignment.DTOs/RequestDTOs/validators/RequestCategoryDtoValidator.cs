using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestCategoryDtoValidator : AbstractValidator<RequestCategoryDto>
{
    private const int MaxLengthNamePropertyLength = 64;
    
    public RequestCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().NotEmpty().WithMessage("Name name is required.")
            .MaximumLength(MaxLengthNamePropertyLength)
            .WithMessage($"Name cannot exceed {MaxLengthNamePropertyLength} characters.");
    }
}