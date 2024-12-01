using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.validators;

namespace HomeAssignment.Persistence.Commands.Attempts.validators;

public class CreateAttemptCommandValidator : AbstractValidator<CreateAttemptCommand>
{
    public CreateAttemptCommandValidator()
    {
        RuleFor(x => x.AttemptDto)
            .NotNull().WithMessage("The attempt object must be passed to the method.")
            .SetValidator(new RequestAttemptDtoValidator());
    }
}