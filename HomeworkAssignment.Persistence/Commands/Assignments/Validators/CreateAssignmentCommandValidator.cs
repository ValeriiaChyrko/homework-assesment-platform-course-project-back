using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.validators;

namespace HomeAssignment.Persistence.Commands.Assignments.Validators;

public class CreateAssignmentCommandValidator : AbstractValidator<CreateAssignmentCommand>
{
    public CreateAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentDto)
            .NotNull().WithMessage("The assignment object must be passed to the method.")
            .SetValidator(new RequestAssignmentDtoValidator());
    }
}