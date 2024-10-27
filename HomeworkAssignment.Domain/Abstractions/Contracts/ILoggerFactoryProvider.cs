namespace HomeAssignment.Domain.Abstractions.Contracts;

public interface ILoggerFactoryProvider
{
    ILoggerFactory GetFactory();
}