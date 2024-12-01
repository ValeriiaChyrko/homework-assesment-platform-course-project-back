using HomeAssignment.Persistence.Abstractions.Errors;

namespace HomeAssignment.Persistence.Abstractions.Exceptions;

public class DatabaseErrorException : Exception
{
    private readonly List<DataBaseError> _errors;

    public DatabaseErrorException(string message, List<DataBaseError> errors, Exception? innerException) : base(message,
        innerException)
    {
        _errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    public Dictionary<string, List<string>> GetErrors()
    {
        var errorsDict = new Dictionary<string, List<string>>();

        if (!_errors.Any())
            errorsDict.Add("Database processing error", new List<string> { "A database operation error occurred." });

        return errorsDict;
    }
}