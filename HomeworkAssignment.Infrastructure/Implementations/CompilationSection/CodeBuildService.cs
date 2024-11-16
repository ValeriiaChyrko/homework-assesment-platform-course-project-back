using System.Diagnostics;
using System.Text;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;

namespace HomeworkAssignment.Infrastructure.Implementations.CompilationSection;

public class CodeBuildService : ICodeBuildService
{
    private readonly ILogger _logger;
    private readonly ILanguageDetector _languageDetector;

    public CodeBuildService(ILogger logger, ILanguageDetector languageDetector)
    {
        _logger = logger;
        _languageDetector = languageDetector;
    }

    public async Task<bool> VerifyProjectCompilation(string repositoryPath, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(repositoryPath))
            throw new DirectoryNotFoundException($"The directory '{repositoryPath}' does not exist.");

        var mainLanguage = _languageDetector.DetectMainLanguage(repositoryPath);
        if (mainLanguage == "Unknown")
        {
            _logger.Log("No supported project files found.");
            return false;
        }

        var projectFiles = GetProjectFilesByLanguage(mainLanguage, repositoryPath);
        if (projectFiles.Length == 0) return false;

        var overallSuccess = true;

        foreach (var projectFile in projectFiles)
        {
            try
            {
                var exitCode = await BuildProjectAsync(projectFile, mainLanguage, cancellationToken);
                if (exitCode != 0) overallSuccess = false;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error building project {projectFile}: {ex.Message}");
                overallSuccess = false;
            }
        }

        return overallSuccess;
    }

    private static string[] GetProjectFilesByLanguage(string language, string repositoryPath)
    {
        return language switch
        {
            "C#" => Directory.GetFiles(repositoryPath, "*.csproj", SearchOption.AllDirectories),
            "Python" => Directory.GetFiles(repositoryPath, "*.py", SearchOption.AllDirectories),
            "Java" => Directory.GetFiles(repositoryPath, "*.java", SearchOption.AllDirectories),
            _ => Array.Empty<string>()
        };
    }

    private static async Task<int> BuildProjectAsync(string projectFile, string language, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(projectFile))
            throw new ArgumentException("Project file cannot be null or empty.", nameof(projectFile));

        var command = GetBuildCommand(language);
        var arguments = GetBuildArguments(language, projectFile);

        var processStartInfo = new ProcessStartInfo
        {
            FileName = command,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;

        var errorBuilder = new StringBuilder();
        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) errorBuilder.AppendLine(e.Data);
        };

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode != 0) throw new Exception($"Error: {errorBuilder}");

            return process.ExitCode;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing the project.", ex);
        }
    }

    private static string GetBuildCommand(string language)
    {
        return language switch
        {
            "C#" => "dotnet",
            "Python" => "python",
            "Java" => "javac",
            _ => throw new NotSupportedException($"Unsupported language: {language}")
        };
    }

    private static string GetBuildArguments(string language, string projectFile)
    {
        return language switch
        {
            "C#" => $"build \"{projectFile}\"",
            "Python" => $"\"{projectFile}\"",
            "Java" => $"{projectFile}",
            _ => throw new NotSupportedException($"Unsupported language: {language}")
        };
    }
}