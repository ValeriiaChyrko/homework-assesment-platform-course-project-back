using System.Diagnostics;
using System.Text;
using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.CompilationSection;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;

namespace HomeworkAssignment.Infrastructure.Implementations.CompilationSection;

public class CodeBuildService : ICodeBuildService
{
    private readonly ILanguageDetector _languageDetector;
    private readonly ILogger _logger;

    public CodeBuildService(ILogger logger, ILanguageDetector languageDetector)
    {
        _logger = logger;
        _languageDetector = languageDetector;
    }

    public async Task<bool> VerifyProjectCompilation(string repositoryPath,
        CancellationToken cancellationToken = default)
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
            try
            {
                var exitCode =
                    await BuildProjectInDockerAsync(projectFile, mainLanguage, repositoryPath, cancellationToken);
                if (exitCode != 0) overallSuccess = false;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error building project {projectFile}: {ex.Message}");
                overallSuccess = false;
            }

        return overallSuccess;
    }

    private static async Task<int> BuildProjectInDockerAsync(string projectFile, string language, string repositoryPath,
    CancellationToken cancellationToken = default)
{
    if (string.IsNullOrWhiteSpace(projectFile))
        throw new ArgumentException("Project file cannot be null or empty.", nameof(projectFile));

    var dockerImage = GetDockerImage(language);
    var command = GetBuildCommand(language);
    
    var relativeProjectPath = Path.GetRelativePath(repositoryPath, projectFile).Replace("\\", "/");
    var fileName = Path.GetFileName(relativeProjectPath);
    var arguments = GetBuildArguments(language, fileName);
    var workingDirectory = GetWorkingDirectory(language, relativeProjectPath);
   
    var dockerCommand = $"docker run -it --rm -v \"{repositoryPath.Replace("\\", "/")}:/workspace\" -w /workspace/{workingDirectory} {dockerImage} {command} {arguments}";
    
    var isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
    var shellFileName = isWindows ? "cmd.exe" : "sh";
    var shellArguments = isWindows ? $"/c \"{dockerCommand}\"" : $"-c \"{dockerCommand}\"";

    var processStartInfo = new ProcessStartInfo
    {
        FileName = shellFileName,
        Arguments = shellArguments,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using var process = new Process();
    process.StartInfo = processStartInfo;

    var outputBuilder = new StringBuilder();
    var errorBuilder = new StringBuilder();

    process.OutputDataReceived += (_, e) =>
    {
        if (!string.IsNullOrEmpty(e.Data)) outputBuilder.AppendLine(e.Data);
    };
    process.ErrorDataReceived += (_, e) =>
    {
        if (!string.IsNullOrEmpty(e.Data)) errorBuilder.AppendLine(e.Data);
    };

    try
    {
        process.Start();
        process.BeginOutputReadLine();

        await process.WaitForExitAsync(cancellationToken);

        var output = outputBuilder.ToString();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Docker build failed.\nOutput:\n{output}\n");
        }

        return process.ExitCode;
    }
    catch (Exception ex)
    {
        throw new Exception("An error occurred while building the project in Docker.", ex);
    }
}


    private static string GetDockerImage(string language)
    {
        return language switch
        {
            "C#" => "mcr.microsoft.com/dotnet/sdk:7.0",
            "Python" => "python:3.11",
            "Java" => "maven:3.8.6-jdk-11",
            _ => throw new NotSupportedException($"Unsupported language: {language}")
        };
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
    
    private static string GetBuildCommand(string language)
    {
        return language switch
        {
            "C#" => "dotnet",
            "Python" => "python3",
            "Java" => "mvn",
            _ => throw new NotSupportedException($"Unsupported language: {language}")
        };
    }

    private static string GetBuildArguments(string language, string projectFile)
    {
        return language switch
        {
            "C#" => $"build {projectFile}",
            "Python" => $"-m py_compile {projectFile}",
            "Java" => string.Empty,
            _ => throw new NotSupportedException($"Unsupported language: {language}")
        };
    }
    
    private static string? GetWorkingDirectory(string language, string relativeProjectPath)
    {
        var workingDirectory = Path.GetDirectoryName(relativeProjectPath)?.Replace("\\", "/");
        
        return language switch
        {
            "C#" => workingDirectory,
            "Python" => workingDirectory,
            "Java" => workingDirectory,
            _ => throw new NotSupportedException($"Unsupported language: {language}")
        };
    }
}