using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestEnrollmentDtoValidator : AbstractValidator<RequestEnrollmentDto>
{
    public RequestEnrollmentDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().NotEmpty().WithMessage("UserId is required.")
            .Must(id => id != Guid.Empty).WithMessage("UserId cannot be an empty GUID.");

        RuleFor(x => x.CourseId)
            .NotNull().NotEmpty().WithMessage("CourseId is required.")
            .Must(id => id != Guid.Empty).WithMessage("CourseId cannot be an empty GUID.");
    }
}