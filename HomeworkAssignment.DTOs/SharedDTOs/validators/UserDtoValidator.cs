using FluentValidation;

namespace HomeAssignment.DTOs.SharedDTOs.validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    private const int MinPasswordLength = 8;
    private const int MaxLengthNamePropertyLength = 128;

    public UserDtoValidator()
    {
        RuleFor(dto => dto.FullName)
            .NotNull().NotEmpty().WithMessage("Full name cannot be empty.")
            .MaximumLength(MaxLengthNamePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthNamePropertyLength} characters.");

        RuleFor(dto => dto.PasswordHash)
            .NotNull().NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(MinPasswordLength)
            .WithMessage($"Password must be at least {MinPasswordLength} characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        RuleFor(dto => dto.Email)
            .NotNull().NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}