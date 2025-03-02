using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestChapterDtoValidator : AbstractValidator<RequestChapterDto>
{
    private const int MaxLengthTitlePropertyLength = 64;
    private const int MaxLengthUrlPropertyLength = 256;
    private const int MaxLengthDescriptionPropertyLength = 512;

    public RequestChapterDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthTitlePropertyLength} characters.");
        
        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .MaximumLength(MaxLengthDescriptionPropertyLength)
            .WithMessage($"Specialization cannot exceed {MaxLengthDescriptionPropertyLength} characters.")
            .When(x => x.Description != null);
        
        RuleFor(dto => dto.VideoUrl)
            .NotEmpty().WithMessage("Video URL cannot be empty.")
            .MaximumLength(MaxLengthUrlPropertyLength)
            .WithMessage($"Video URL cannot exceed {MaxLengthUrlPropertyLength} characters.")
            .When(x => x.VideoUrl != null);

        RuleFor(dto => dto.Position)
            .NotEmpty().WithMessage("The value of max score cannot be empty.")
            .GreaterThan(0).WithMessage("The value of max score must be greater than one.");

        RuleFor(x => x.MuxDataId)
            .NotEmpty().WithMessage("MuxDataId is required.")
            .Must(id => id != Guid.Empty).WithMessage("MuxDataId cannot be an empty GUID.")
            .When(x => x.MuxDataId != null);
        
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("CourseId is required.")
            .Must(id => id != Guid.Empty).WithMessage("CourseId cannot be an empty GUID.")
            .When(x => x.CourseId != null);
    }
}