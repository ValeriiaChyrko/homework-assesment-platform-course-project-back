using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestUserDtoValidator : AbstractValidator<RequestUserDto>
{
    private const int MaxLengthFullNamePropertyLength = 128;
    private const int MaxLengthGithubUsernamePropertyLength = 64;
    private const int MinPasswordLength = 8;
    
    public RequestUserDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full Name is required.")
            .MaximumLength(MaxLengthFullNamePropertyLength).WithMessage($"Full Name must not exceed {MaxLengthFullNamePropertyLength} characters.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");
        
        RuleFor(x => x.Password)
            .NotNull().NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(MinPasswordLength)
            .WithMessage($"Password must be at least {MinPasswordLength} characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.GithubUsername)
            .NotEmpty().WithMessage("GitHub Username is required.")
            .MaximumLength(MaxLengthGithubUsernamePropertyLength).WithMessage($"GitHub Username must not exceed {MaxLengthGithubUsernamePropertyLength} characters.");
        
        RuleFor(x => x.GithubProfileUrl)
            .NotEmpty().WithMessage("GitHub Profile URL is required.")
            .Must(BeAValidUrl).WithMessage("GitHub Profile URL must be a valid URL.");
        
        RuleFor(x => x.GithubPictureUrl)
            .Must(BeAValidUrl!).WithMessage("GitHub Picture URL must be a valid URL if provided.")
            .When(x => !string.IsNullOrEmpty(x.GithubPictureUrl));
    }
    
    private static bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}