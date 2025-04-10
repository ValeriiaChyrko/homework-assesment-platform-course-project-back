using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestPartialCourseDtoValidator : AbstractValidator<RequestPartialCourseDto>
{
    private const int MaxLengthTitlePropertyLength = 64;
    private const int MaxLengthDescriptionPropertyLength = 512;
    private const int MaxLengthUrlPropertyLength = 256;
    
    public RequestPartialCourseDtoValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.")
            .Must(id => id != Guid.Empty).WithMessage("CategoryId cannot be an empty GUID.")
            .When(x => x.CategoryId != null);

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Title cannot exceed {MaxLengthTitlePropertyLength} characters.")
            .When(x => x.Title != null);
        
        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .MaximumLength(MaxLengthDescriptionPropertyLength)
            .WithMessage($"Specialization cannot exceed {MaxLengthDescriptionPropertyLength} characters.")
            .When(x => x.Description != null);
        
        RuleFor(dto => dto.ImageUrl)
            .NotEmpty().WithMessage("URL must be a valid property name.")
            .MaximumLength(MaxLengthUrlPropertyLength)
            .WithMessage($"URL cannot exceed {MaxLengthUrlPropertyLength} characters.")
            .When(x => x.ImageUrl != null);
    }
}