using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestSingleCourseFilterParametersValidator : AbstractValidator<RequestSingleCourseFilterParameters>
{
    
    public RequestSingleCourseFilterParametersValidator()
    {
        RuleFor(x => x.Include)
            .Must(include => include == null || include.All(IsValidInclude)) 
            .WithMessage("Invalid Include property");
    }
    
    private static readonly HashSet<string> ValidIncludes =
    [
        "chapters",
        "attachments"
    ];
    
    private static bool IsValidInclude(string value)
    {
        return ValidIncludes.Contains(value);
    }
}