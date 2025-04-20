using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.ChapterRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestChapterFilterParametersValidator : AbstractValidator<RequestChapterFilterParameters>
{
    private const int MaxLengthTitlePropertyLength = 64;

    private static readonly HashSet<string> ValidProperties =
    [
        "Title",
        "CourseId",
        "Position",
        "RepositoryName"
    ];

    public RequestChapterFilterParametersValidator()
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthTitlePropertyLength} characters.")
            .When(dto => dto.Title != null);

        RuleFor(x => x.CourseId)
            .Must(id => id != Guid.Empty).WithMessage("CourseId cannot be an empty GUID.")
            .When(dto => dto.CourseId != null);

        RuleFor(x => x.SortBy)
            .Must(BeAValidPropertyName!).WithMessage("SortBy must be a valid property name.")
            .When(dto => dto.SortBy != null);

        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
    }

    private static bool BeAValidPropertyName(string sortBy)
    {
        return ValidProperties.Contains(sortBy);
    }
}