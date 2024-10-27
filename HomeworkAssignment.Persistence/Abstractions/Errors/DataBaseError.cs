namespace HomeAssignment.Persistence.Abstractions.Errors;

public class DataBaseError
{
    public string Table { get; init; } = string.Empty;
    public string? Message { get; init; } = string.Empty;
}