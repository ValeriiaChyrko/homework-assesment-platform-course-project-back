using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestCourseFilterParametersValidator : AbstractValidator<RequestCourseFilterParameters>
{
    private const int MaxLengthTitlePropertyLength = 64;

    private static readonly HashSet<string> ValidProperties =
    [
        "Title",
        "OwnerId",
        "Deadline",
        "CreatedAt",
        "RepositoryName"
    ];

    private static readonly HashSet<string> ValidIncludes =
    [
        "category",
        "progress"
    ];

    public RequestCourseFilterParametersValidator()
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthTitlePropertyLength} characters.")
            .When(dto => dto.Title != null);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
    }

    private static bool BeAValidPropertyName(string sortBy)
    {
        return ValidProperties.Contains(sortBy);
    }

    private static bool IsValidInclude(string value)
    {
        return ValidIncludes.Contains(value);
    }
}