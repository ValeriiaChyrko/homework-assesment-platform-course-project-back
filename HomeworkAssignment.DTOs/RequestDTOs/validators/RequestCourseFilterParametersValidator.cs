using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestCourseFilterParametersValidator : AbstractValidator<RequestCourseFilterParameters>
{
    private const int MaxLengthTitlePropertyLength = 64;
    
    public RequestCourseFilterParametersValidator()
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthTitlePropertyLength} characters.")
            .When(dto => dto.Title != null);
        
        RuleFor(x => x.OwnerId)
            .Must(id => id != Guid.Empty).WithMessage("OwnerId cannot be an empty GUID.")
            .When(dto => dto.OwnerId != null);
        
        RuleFor(x => x.SortBy)
            .Must(BeAValidPropertyName!).WithMessage("SortBy must be a valid property name.")
            .When(dto => dto.SortBy != null);
        
        RuleFor(x => x.Include)
            .Must(include => include == null || include.All(IsValidInclude)) 
            .WithMessage("Invalid Include property");
        
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");
        
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
    }
    
    private static readonly HashSet<string> ValidProperties =
    [
        "Title",
        "OwnerId",
        "Deadline",
        "CreatedAt",
        "RepositoryName"
    ];

    private static bool BeAValidPropertyName(string sortBy)
    {
        return ValidProperties.Contains(sortBy);
    }
    
    private static readonly HashSet<string> ValidIncludes =
    [
        "category",
        "progress"
    ];
    
    private static bool IsValidInclude(string value)
    {
        return ValidIncludes.Contains(value);
    }
}