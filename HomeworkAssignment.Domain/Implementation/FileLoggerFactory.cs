using HomeAssignment.Domain.Abstractions.Contracts;

namespace HomeAssignment.Domain.Implementation;

public class FileLoggerFactory : ILoggerFactory
{
    private readonly string _filePath;

    public FileLoggerFactory(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath)) filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

        if (!File.Exists(filePath)) File.Create(filePath).Dispose();

        _filePath = filePath;
    }

    public ILogger CreateLogger()
    {
        return new FileLogger(_filePath);
    }
}