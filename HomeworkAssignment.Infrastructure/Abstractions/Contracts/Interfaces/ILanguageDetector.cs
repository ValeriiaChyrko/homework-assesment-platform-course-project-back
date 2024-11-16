namespace HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;

public interface ILanguageDetector
{
    string DetectMainLanguage(string repositoryPath);
}