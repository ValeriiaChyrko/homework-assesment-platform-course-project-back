using FluentValidation;

namespace HomeAssignment.Persistence.Commands.Users.Validators;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The User profile Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("The User profile Id cannot be an empty GUID.");
    }
}