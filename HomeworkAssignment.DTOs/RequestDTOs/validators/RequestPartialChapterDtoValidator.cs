using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.ChapterRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestPartialChapterDtoValidator : AbstractValidator<RequestPartialChapterDto>
{
    private const int MaxLengthTitlePropertyLength = 64;
    private const int MaxLengthUrlPropertyLength = 256;
    private const int MaxLengthDescriptionPropertyLength = 10000;

    public RequestPartialChapterDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthTitlePropertyLength} characters.")
            .When(dto => dto.Title != null);

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
            .GreaterThan(-1).WithMessage("The value of position must be greater than zero.");
    }
}