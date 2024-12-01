using System.Text.RegularExpressions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;
using HomeworkAssignment.Infrastructure.Abstractions.TestsSection;

namespace HomeworkAssignment.Infrastructure.Implementations.TestsSection;

public partial class JavaTestsRunner : ITestsRunner
{
    private const string DockerImage = "maven:3.8.6-openjdk-11";
    private const string Command = "mvn";
    private static readonly Regex PassedPattern = GeneratePassedPatternRegex();
    private static readonly Regex FailedPattern = GenerateFailedPatternRegex();
    private readonly IDockerService _dockerService;

    private readonly ILogger _logger;

    public JavaTestsRunner(ILogger logger, IDockerService dockerService)
    {
        _logger = logger;
        _dockerService = dockerService;
    }

    public async Task<IEnumerable<TestResult>> RunTestsAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        var testResults = new List<TestResult>();

        try
        {
            var resultSet = await RunTestsInDockerAsync(repositoryPath, cancellationToken);
            testResults.AddRange(resultSet);
        }
        catch (Exception ex)
        {
            _logger.Log($"Error running tests for {repositoryPath}: {ex.Message}");
        }

        return testResults;
    }

    private async Task<List<TestResult>> RunTestsInDockerAsync(string repositoryPath,
        CancellationToken cancellationToken)
    {
        const string arguments = "test";

        var result = await _dockerService.RunCommandAsync(
            repositoryPath,
            string.Empty,
            DockerImage,
            Command,
            arguments,
            cancellationToken
        );

        return ParseTestResults(result.OutputDataReceived);
    }

    private static List<TestResult> ParseTestResults(string output)
    {
        var regex = CompileTestExecutionOutputRegex();
        var match = regex.Match(output);

        var filteredOutput = match.Success ? match.Groups[1].Value : string.Empty;

        var lines = filteredOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        return lines.Select(ParseTestResult).OfType<TestResult>().ToList();
    }

    private static TestResult? ParseTestResult(string line)
    {
        var match = PassedPattern.Match(line);
        if (match.Success)
            return new TestResult
            {
                TestName = match.Groups["TestName"].Value,
                IsPassed = true,
                ExecutionTimeMs = ExtractExecutionTime(match.Groups["Time"].Value)
            };

        match = FailedPattern.Match(line);
        if (match.Success)
            return new TestResult
            {
                TestName = match.Groups["TestName"].Value,
                IsPassed = false,
                ExecutionTimeMs = ExtractExecutionTime(match.Groups["Time"].Value)
            };

        return null;
    }

    private static double ExtractExecutionTime(string timeOutput)
    {
        var cleanTime = timeOutput.Replace("<", "").Replace(" ms", "").Trim();
        return double.TryParse(cleanTime, out var time) ? time : 0;
    }

    [GeneratedRegex(@"(?<TestName>[\w\.]+)\s+\(\d+\s+ms\)\s+Success", RegexOptions.Compiled)]
    private static partial Regex GeneratePassedPatternRegex();

    [GeneratedRegex(@"(?<TestName>[\w\.]+)\s+\(\d+\s+ms\)\s+Failed", RegexOptions.Compiled)]
    private static partial Regex GenerateFailedPatternRegex();

    [GeneratedRegex(@"(?s).*?TESTS(.*)", RegexOptions.IgnoreCase, "uk-UA")]
    private static partial Regex CompileTestExecutionOutputRegex();
}