using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestSingleCourseFilterParametersValidator : AbstractValidator<RequestSingleCourseFilterParameters>
{
    private static readonly HashSet<string> ValidIncludes =
    [
        "chapters",
        "attachments"
    ];

    public RequestSingleCourseFilterParametersValidator()
    {
        RuleFor(x => x.Include)
            .Must(include => include == null || include.All(IsValidInclude))
            .WithMessage("Invalid Include property");
    }

    private static bool IsValidInclude(string value)
    {
        return ValidIncludes.Contains(value);
    }
}