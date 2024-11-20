using System.Diagnostics;
using System.Text.RegularExpressions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.TestsSection;

namespace HomeworkAssignment.Infrastructure.Implementations.TestsSection;

public class PythonTestsRunner : ITestsRunner
{
    private const string PassedPattern = "(OK|PASSED)";
    private const string FailedPattern = "(FAIL|FAILED)";

    private readonly ILogger _logger;

    public PythonTestsRunner(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<TestResult>> RunTestsAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        var testsDirectory = Path.Combine(repositoryPath, "tests");
        var testFiles = Directory.GetFiles(testsDirectory, "*.py", SearchOption.AllDirectories);
        var testResults = new List<TestResult>();

        foreach (var testFile in testFiles)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "python3",
                Arguments = $"-m unittest \"{testFile}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = processStartInfo;

            process.OutputDataReceived += (_, args) =>
            {
                if (string.IsNullOrEmpty(args.Data)) return;

                if (Regex.IsMatch(args.Data, PassedPattern, RegexOptions.IgnoreCase))
                    testResults.Add(new TestResult
                    {
                        TestName = ExtractTestName(args.Data),
                        IsPassed = true,
                        ExecutionTimeMs = ExtractExecutionTime(args.Data)
                    });
                else if (Regex.IsMatch(args.Data, FailedPattern, RegexOptions.IgnoreCase))
                    testResults.Add(new TestResult
                    {
                        TestName = ExtractTestName(args.Data),
                        IsPassed = false,
                        ExecutionTimeMs = ExtractExecutionTime(args.Data)
                    });
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

    private static string ExtractTestName(string output)
    {
        var match = Regex.Match(output, @"test_(?<TestName>\w+)", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups["TestName"].Value : "Unknown";
    }

    private static double ExtractExecutionTime(string output)
    {
        var match = Regex.Match(output, @"(?<Time>\d+(\.\d+)?)s", RegexOptions.IgnoreCase);
        return match.Success && double.TryParse(match.Groups["Time"].Value, out var time) ? time * 1000 : 0;
    }
}