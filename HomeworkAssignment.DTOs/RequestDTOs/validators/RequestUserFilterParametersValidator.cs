using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestUserFilterParametersValidator : AbstractValidator<RequestUserFilterParameters>
{
    private const int MaxLengthFullNamePropertyLength = 128;
    private const int MaxLengthGithubUsernamePropertyLength = 64;
    
    public RequestUserFilterParametersValidator()
    {
        RuleFor(x => x.FullName)
            .MaximumLength(MaxLengthFullNamePropertyLength).WithMessage($"Full Name must not exceed {MaxLengthFullNamePropertyLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FullName));
        
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
        
        RuleFor(x => x.SortBy)
            .Must(BeAValidPropertyName!).WithMessage("SortBy must be a valid property name.")
            .When(dto => dto.SortBy != null);
        
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
        
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
    }
    
    private static readonly HashSet<string> ValidProperties =
    [
        "FullName",
        "Email"
    ];

    private static bool BeAValidPropertyName(string sortBy)
    {
        return ValidProperties.Contains(sortBy);
    }
}