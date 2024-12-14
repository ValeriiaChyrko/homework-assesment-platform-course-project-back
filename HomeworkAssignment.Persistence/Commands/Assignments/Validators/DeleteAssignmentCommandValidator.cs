using FluentValidation;

namespace HomeAssignment.Persistence.Commands.Assignments.Validators;

public class DeleteAssignmentCommandValidator : AbstractValidator<DeleteAssignmentCommand>
{
    public DeleteAssignmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The assignment Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("The assignment Id cannot be an empty GUID.");
    }
}