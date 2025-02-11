using HomeAssignment.DTOs.RequestDTOs;

namespace HomeworkAssignment.Services.Abstractions;

public interface ITestsGrpcService
{
    Task<int> VerifyProjectPassedTestsAsync(RequestRepositoryWithBranchDto query,
        CancellationToken cancellationToken = default);
}