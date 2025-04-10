using FluentValidation;
using HomeAssignment.DTOs.RequestDTOs.AssignmentRelated;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestAssignmentDtoValidator : AbstractValidator<RequestCreateAssignmentDto>
{
    private const int MaxLengthTitlePropertyLength = 64;
    public RequestAssignmentDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(MaxLengthTitlePropertyLength)
            .WithMessage($"Full name cannot exceed {MaxLengthTitlePropertyLength} characters.");
    }
}