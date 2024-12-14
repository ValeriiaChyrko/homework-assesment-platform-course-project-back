using FluentValidation;
using HomeAssignment.DTOs.SharedDTOs.validators;

namespace HomeAssignment.Persistence.Commands.Users.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserDto)
            .NotNull().WithMessage("The User profile object must be passed to the method.")
            .SetValidator(new UserDtoValidator());
    }
}