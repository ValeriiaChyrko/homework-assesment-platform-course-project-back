using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestStudentDtoValidator : AbstractValidator<RequestStudentDto>
{
    private const int MinPasswordLength = 8;
    private const int MaxLengthNamePropertyLength = 128;
    private const int MaxLengthUserNamePropertyLength = 64;
    private const int MaxLengthAccessTokenPropertyLength = 128;
    private const int MaxLengthUrlPropertyLength = 128;
    
    public RequestStudentDtoValidator()
    {
        RuleFor(dto => dto.FullName)
            .NotNull().NotEmpty().WithMessage("Full name cannot be empty.")
            .MaximumLength(MaxLengthNamePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthNamePropertyLength} characters.");
        
        RuleFor(dto => dto.Password)
            .NotNull().NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(MinPasswordLength).WithMessage($"Password must be at least {MinPasswordLength} characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
        
        RuleFor(dto => dto.Email)
            .NotNull().NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email format.");
        
        RuleFor(dto => dto.GithubUsername)
            .NotNull().NotEmpty().WithMessage("Github username cannot be empty.")
            .MaximumLength(MaxLengthUserNamePropertyLength)
            .WithMessage($"Github username  cannot exceed {MaxLengthUserNamePropertyLength} characters.");
        
        RuleFor(dto => dto.GithubAccessToken)
            .NotNull().NotEmpty().WithMessage("Github access token cannot be empty.")
            .MaximumLength(MaxLengthAccessTokenPropertyLength)
            .WithMessage($"Github access token cannot exceed {MaxLengthAccessTokenPropertyLength} characters.");
        
        RuleFor(dto => dto.GithubProfileUrl)
            .NotNull().NotEmpty().WithMessage("Github profile url cannot be empty.")
            .MaximumLength(MaxLengthUrlPropertyLength)
            .WithMessage($"Github profile url cannot exceed {MaxLengthUrlPropertyLength} characters.");
        
        RuleFor(dto => dto.GithubPictureUrl)
            .NotEmpty().WithMessage("Github picture url cannot be empty.")
            .MaximumLength(MaxLengthUrlPropertyLength)
            .WithMessage($"Github picture url cannot exceed {MaxLengthUrlPropertyLength} characters.")
            .When(dto => dto.GithubPictureUrl != null);
    }
}