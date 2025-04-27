using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.CourseRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestCourseDtoValidator : AbstractValidator<RequestCreateCourseDto>
{
    private const int MaxLengthTitlePropertyLength = 64;
    private const int MaxLengthDescriptionPropertyLength = 512;
    private const int MaxLengthUrlPropertyLength = 256;

    public RequestCourseDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotNull().NotEmpty().WithMessage("Title is required.")
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Title cannot exceed {MaxLengthTitlePropertyLength} characters.");
    }
}