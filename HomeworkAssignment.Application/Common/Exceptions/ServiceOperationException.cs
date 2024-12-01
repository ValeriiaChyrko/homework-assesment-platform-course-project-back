namespace HomeworkAssignment.Application.Common.Exceptions;

public class ServiceOperationException : Exception
{
    public ServiceOperationException(string message, Exception? innerException = null) 
        : base(message, innerException) { }
}