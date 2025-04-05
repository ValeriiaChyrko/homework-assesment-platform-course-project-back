using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestChapterDtoValidator : AbstractValidator<RequestCreateChapterDto>
{
    private const int MaxLengthTitlePropertyLength = 64;
    private const int MaxLengthUrlPropertyLength = 256;
    private const int MaxLengthDescriptionPropertyLength = 10000;

    public RequestChapterDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthTitlePropertyLength} characters.");
    }
}