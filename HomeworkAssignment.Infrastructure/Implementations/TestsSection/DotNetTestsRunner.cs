using System.Diagnostics;
using System.Text.RegularExpressions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.TestsSection;

namespace HomeworkAssignment.Infrastructure.Implementations.TestsSection;

public class DotNetTestsRunner : ITestsRunner
{
    private const string PassedPattern = @"Passed\s+(?<TestName>[\w\.]+)\s+\[(?<Time>\d+(\.\d+)?\s+ms)\]";
    private const string FailedPattern = @"Failed\s+(?<TestName>[\w\.]+)\s+\[(?<Time>\d+(\.\d+)?\s+ms)\]";

    private readonly ILogger _logger;

    public DotNetTestsRunner(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<TestResult>> RunTestsAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        var testsDirectory = Path.Combine(repositoryPath, "tests");
        var testFiles = Directory.GetFiles(testsDirectory, "*.csproj", SearchOption.AllDirectories);
        var testResults = new List<TestResult>();

        foreach (var testFile in testFiles)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"test \"{testFile}\" --verbosity normal",
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
                        ExecutionTimeMs = ExtractExecutionTime(match.Groups["Time"].Value)
                    });
                }
                else if (Regex.IsMatch(args.Data, FailedPattern))
                {
                    var match = Regex.Match(args.Data, FailedPattern);
                    testResults.Add(new TestResult
                    {
                        TestName = match.Groups["TestName"].Value,
                        IsPassed = false,
                        ExecutionTimeMs = ExtractExecutionTime(match.Groups["Time"].Value)
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
                _logger.Log($"Error running tests for {testFile}: {ex.Message}");
            }
        }

        return testResults;
    }

    private static double ExtractExecutionTime(string timeOutput)
    {
        if (double.TryParse(timeOutput.Replace(" ms", ""), out var time)) return time;
        return 0;
    }
}