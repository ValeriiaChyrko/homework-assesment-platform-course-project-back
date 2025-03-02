using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestUserProgressDtoValidator : AbstractValidator<RequestUserProgressDto>
{
    public RequestUserProgressDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().NotEmpty().WithMessage("UserId is required.")
            .Must(id => id != Guid.Empty).WithMessage("UserId cannot be an empty GUID.");

        RuleFor(x => x.ChapterId)
            .NotNull().NotEmpty().WithMessage("ChapterId is required.")
            .Must(id => id != Guid.Empty).WithMessage("ChapterId cannot be an empty GUID.");
    }
}