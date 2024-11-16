using HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;

namespace HomeworkAssignment.Infrastructure.Implementations.Helpers;

public class LanguageDetector : ILanguageDetector
{
    private static readonly Dictionary<string, string> LanguageExtensions = new()
    {
        { ".cs", "C#" },
        { ".csproj", "C#" },
        { ".py", "Python" },
        { ".java", "Java" }
    };

    public string DetectMainLanguage(string repositoryPath)
    {
        if (!Directory.Exists(repositoryPath))
            throw new DirectoryNotFoundException($"The directory '{repositoryPath}' does not exist.");

        var fileCounts = new Dictionary<string, int>();
        var files = Directory.GetFiles(repositoryPath, "*.*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var extension = Path.GetExtension(file).ToLower();
            if (!LanguageExtensions.TryGetValue(extension, out var language)) continue;
            fileCounts.TryAdd(language, 0);
            fileCounts[language]++;
        }

        return fileCounts.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key ?? "Unknown";
    }
}