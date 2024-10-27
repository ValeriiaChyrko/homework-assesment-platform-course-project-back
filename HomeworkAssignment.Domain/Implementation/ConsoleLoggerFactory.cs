using HomeAssignment.Domain.Abstractions.Contracts;

namespace HomeAssignment.Domain.Implementation;

public class ConsoleLoggerFactory : ILoggerFactory
{
    public ILogger CreateLogger()
    {
        return new ConsoleLogger();
    }
}