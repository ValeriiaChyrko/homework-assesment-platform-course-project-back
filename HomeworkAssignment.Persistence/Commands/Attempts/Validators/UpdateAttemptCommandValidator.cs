using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.validators;

namespace HomeAssignment.Persistence.Commands.Attempts.Validators;

public class UpdateAttemptCommandValidator : AbstractValidator<UpdateAttemptCommand>
{
    public UpdateAttemptCommandValidator()
    {
        RuleFor(x => x.AttemptDto)
            .NotNull().WithMessage("The attempt object must be passed to the method.")
            .SetValidator(new RequestAttemptDtoValidator());

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The attempt Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("The attempt Id cannot be an empty GUID.");
    }
}