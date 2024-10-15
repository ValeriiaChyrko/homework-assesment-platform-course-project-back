namespace HomeAssignment.Domain.Abstractions.Contracts;

public interface ILoggerFactory
{
    ILogger CreateLogger();
}