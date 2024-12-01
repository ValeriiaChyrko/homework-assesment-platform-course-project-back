using System.Text.RegularExpressions;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.DockerRelated;
using HomeworkAssignment.Infrastructure.Abstractions.TestsSection;

namespace HomeworkAssignment.Infrastructure.Implementations.TestsSection;

public partial class DotNetTestsRunner : ITestsRunner
{
    private const string DockerImage = "mcr.microsoft.com/dotnet/sdk:7.0";
    private const string Command = "dotnet";
    private static readonly Regex PassedPattern = GeneratePassedPatternRegex();
    private static readonly Regex FailedPattern = GenerateFailedPatternRegex();
    private readonly IDockerService _dockerService;

    private readonly ILogger _logger;

    public DotNetTestsRunner(ILogger logger, IDockerService dockerService)
    {
        _logger = logger;
        _dockerService = dockerService;
    }

    public async Task<IEnumerable<TestResult>> RunTestsAsync(string repositoryPath, CancellationToken cancellationToken)
    {
        var testFiles =
            Directory.GetFiles(Path.Combine(repositoryPath, "tests"), "*.csproj", SearchOption.AllDirectories);
        var testResults = new List<TestResult>();

        foreach (var testFile in testFiles)
            try
            {
                var resultSet = await RunTestsInDockerAsync(testFile, repositoryPath, cancellationToken);
                testResults.AddRange(resultSet);
            }
            catch (Exception ex)
            {
                _logger.Log($"Error running tests for {testFile}: {ex.Message}");
            }

        return testResults;
    }

    private async Task<List<TestResult>> RunTestsInDockerAsync(
        string testFile,
        string repositoryPath,
        CancellationToken cancellationToken)
    {
        var relativePath = Path.GetRelativePath(repositoryPath, testFile);
        var arguments = $"test \"{Path.GetFileName(relativePath)}\" --verbosity normal";

        var workingDirectory = Path.GetDirectoryName(relativePath) ?? string.Empty;

        var result = await _dockerService.RunCommandAsync(
            repositoryPath,
            workingDirectory,
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

    [GeneratedRegex(@"Passed\s+(?<TestName>[^\s]+)\s+\[\<\s+(?<Time>\d+(\.\d+)?\s+ms)\]", RegexOptions.Compiled)]
    private static partial Regex GeneratePassedPatternRegex();

    [GeneratedRegex(@"Failed\s+(?<TestName>[^\s]+)\s+\[(?<Time>\d+(\.\d+)?\s+ms)\]", RegexOptions.Compiled)]
    private static partial Regex GenerateFailedPatternRegex();

    [GeneratedRegex(@"(?s).*?Starting test execution, please wait\.\.\.(.*)", RegexOptions.IgnoreCase, "uk-UA")]
    private static partial Regex CompileTestExecutionOutputRegex();
}