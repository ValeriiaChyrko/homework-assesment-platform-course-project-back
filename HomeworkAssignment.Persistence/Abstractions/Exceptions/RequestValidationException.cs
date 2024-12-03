using HomeAssignment.Persistence.Abstractions.Errors;

namespace HomeAssignment.Persistence.Abstractions.Exceptions;

public class RequestValidationException : Exception
{
    private readonly List<ValidationError> _errors;

    public RequestValidationException(string message, List<ValidationError> errors, Exception innerException)
        : base(message, innerException)
    {
        _errors = errors ?? throw new ArgumentNullException(nameof(errors));

        if (!_errors.Any()) throw new ArgumentException("Error list cannot be empty.", nameof(errors));
    }

    public Dictionary<string, List<string>> GetErrors()
    {
        var errorsDict = new Dictionary<string, List<string>>();

        foreach (var error in _errors)
        {
            var key = $"Field {error.Field} is invalid";

            if (!errorsDict.TryGetValue(key, out var messages))
            {
                messages = new List<string>();
                errorsDict[key] = messages;
            }

            messages.Add(error.Message);
        }

        return errorsDict;
    }
}