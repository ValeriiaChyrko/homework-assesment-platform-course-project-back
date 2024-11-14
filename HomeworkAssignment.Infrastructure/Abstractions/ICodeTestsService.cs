using HomeworkAssignment.Infrastructure.Abstractions.Contracts;

namespace HomeworkAssignment.Infrastructure.Abstractions;

public interface ICodeTestsService
{
    Task<int> CheckCodeTestsAsync(string testsDirectory, CancellationToken cancellationToken = default);
}