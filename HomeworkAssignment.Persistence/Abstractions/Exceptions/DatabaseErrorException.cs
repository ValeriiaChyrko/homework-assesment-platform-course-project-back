using HomeAssignment.Persistence.Abstractions.Errors;

namespace HomeAssignment.Persistence.Abstractions.Exceptions;

public class DatabaseErrorException : Exception
{
    public List<DataBaseError> Errors { get; }

    public DatabaseErrorException(string message, List<DataBaseError> errors, Exception? innerException) : base(message,
        innerException)
    {
        Errors = errors;
    }
}