using FluentValidation;

namespace HomeAssignment.DTOs.RequestDTOs.validators;

public class RequestMuxDataDtoValidator : AbstractValidator<RequestMuxDataDto>
{
    public RequestMuxDataDtoValidator()
    {
        RuleFor(x => x.ChapterId)
            .NotNull().NotEmpty().WithMessage("ChapterId is required.")
            .Must(id => id != Guid.Empty).WithMessage("ChapterId cannot be an empty GUID.");

        RuleFor(x => x.AssetId)
            .NotNull().NotEmpty().WithMessage("AssetId is required.");
        
        RuleFor(x => x.PlaybackId)
            .NotNull().NotEmpty().WithMessage("PlaybackId is required.");
    }
}