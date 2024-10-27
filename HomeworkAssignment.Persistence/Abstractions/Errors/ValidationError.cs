namespace HomeAssignment.Persistence.Abstractions.Errors;

public class ValidationError
{
    public string Entity { get; init; } = string.Empty;
    public string Field { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}