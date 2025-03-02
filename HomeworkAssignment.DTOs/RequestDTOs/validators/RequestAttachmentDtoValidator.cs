using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestAttachmentDtoValidator : AbstractValidator<RequestAttachmentDto>
{
    private const int MaxLengthNamePropertyLength = 64;
    private const int MaxLengthUrlPropertyLength = 256;
    
    public RequestAttachmentDtoValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("CourseId is required.")
            .Must(id => id != Guid.Empty).WithMessage("CourseId cannot be an empty GUID.")
            .When(x => x.CourseId != null);

        RuleFor(x => x.ChapterId)
            .NotEmpty().WithMessage("ChapterId is required.")
            .Must(id => id != Guid.Empty).WithMessage("ChapterId cannot be an empty GUID.")
            .When(x => x.ChapterId != null);

        RuleFor(x => x.Name)
            .NotNull().NotEmpty().WithMessage("Name name is required.")
            .MaximumLength(MaxLengthNamePropertyLength)
            .WithMessage($"Name cannot exceed {MaxLengthNamePropertyLength} characters.");
        
        RuleFor(dto => dto.Url)
            .NotNull().NotEmpty().WithMessage("URL must be a valid property name.")
            .MaximumLength(MaxLengthUrlPropertyLength)
            .WithMessage($"URL cannot exceed {MaxLengthUrlPropertyLength} characters.");
    }
}