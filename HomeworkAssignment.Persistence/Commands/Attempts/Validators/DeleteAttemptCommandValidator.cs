using FluentValidation;

namespace HomeAssignment.Persistence.Commands.Attempts.Validators;

public class DeleteAttemptCommandValidator : AbstractValidator<DeleteAttemptCommand>
{
    public DeleteAttemptCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The attempt Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("The attempt Id cannot be an empty GUID.");
    }
}