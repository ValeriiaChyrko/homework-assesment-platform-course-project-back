using FluentValidation;
using HomeAssignment.DTOs.SharedDTOs.validators;

namespace HomeAssignment.Persistence.Commands.Users.validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserDto)
            .NotNull().WithMessage("The User profile object must be passed to the method.")
            .SetValidator(new UserDtoValidator());
    }
}