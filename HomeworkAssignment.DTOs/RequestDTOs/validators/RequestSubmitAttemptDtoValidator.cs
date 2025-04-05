using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestSubmitAttemptDtoValidator : AbstractValidator<RequestSubmitAttemptDto>
{
    public RequestSubmitAttemptDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(id => id != Guid.Empty).WithMessage("UserId cannot be an empty GUID.");
    }
}