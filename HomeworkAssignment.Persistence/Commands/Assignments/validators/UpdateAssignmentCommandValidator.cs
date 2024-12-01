using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.validators;

namespace HomeAssignment.Persistence.Commands.Assignments.validators;

public class UpdateAssignmentCommandValidator : AbstractValidator<UpdateAssignmentCommand>
{
    public UpdateAssignmentCommandValidator()
    {
        RuleFor(x => x.AssignmentDto)
            .NotNull().WithMessage("The assignment object must be passed to the method.")
            .SetValidator(new RequestAssignmentDtoValidator());
        
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("The assignment Id is required.")
            .Must(id => id != Guid.Empty).WithMessage("The assignment Id cannot be an empty GUID.");
    }
}