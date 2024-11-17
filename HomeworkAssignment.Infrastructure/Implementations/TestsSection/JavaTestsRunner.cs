using System.Diagnostics;
using System.Text.RegularExpressions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.TestsSection;
using InvalidOperationException = System.InvalidOperationException;

namespace HomeworkAssignment.Infrastructure.Implementations.TestsSection;

public class JavaTestsRunner : ITestsRunner
{
    private const string PassedPattern = @"(?<TestName>[\w\.]+)\s+\(\d+\s+ms\)\s+Success";
    private const string FailedPattern = @"(?<TestName>[\w\.]+)\s+\(\d+\s+ms\)\s+Failed";

    private readonly ILogger _logger;

    public JavaTestsRunner(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<TestResult>> RunTestsAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        var testResults = new List<TestResult>();
        var mvnPath = Environment.GetEnvironmentVariable("PATH")?.Split(';')
            .FirstOrDefault(p => File.Exists(Path.Combine(p, "mvn.cmd")));

        if (mvnPath == null)
        {
            throw new InvalidOperationException("Maven is not installed or is not available in PATH. Please ensure Maven is installed and added to PATH.");
        }

        var processStartInfo = new ProcessStartInfo
        {
            FileName = mvnPath, 
            Arguments = "test",
            WorkingDirectory = repositoryPath,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;

        process.OutputDataReceived += (_, args) =>
        {
            if (string.IsNullOrEmpty(args.Data)) return;

            if (Regex.IsMatch(args.Data, PassedPattern))
            {
                var match = Regex.Match(args.Data, PassedPattern);
                testResults.Add(new TestResult
                {
                    TestName = match.Groups["TestName"].Value,
                    IsPassed = true,
                    ExecutionTimeMs = ExtractExecutionTime(args.Data)
                });
            }
            else if (Regex.IsMatch(args.Data, FailedPattern))
            {
                var match = Regex.Match(args.Data, FailedPattern);
                testResults.Add(new TestResult
                {
                    TestName = match.Groups["TestName"].Value,
                    IsPassed = false,
                    ExecutionTimeMs = ExtractExecutionTime(args.Data)
                });
            }
        };

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            await process.WaitForExitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Log($"Error running tests for {repositoryPath}: {ex.Message}");
        }

        return testResults;
    }

    private static double ExtractExecutionTime(string output)
    {
        const string timePattern = @"\((?<Time>\d+)\s+ms\)";
        var match = Regex.Match(output, timePattern);
        if (match.Success && double.TryParse(match.Groups["Time"].Value, out var time))
        {
            return time;
        }
        return 0;
    }
}