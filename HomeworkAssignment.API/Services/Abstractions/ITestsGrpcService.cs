using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

namespace HomeworkAssignment.Services.Abstractions;

public interface ITestsGrpcService
{
    Task<int> VerifyProjectPassedTestsAsync(RequestRepositoryWithBranchDto query,
        CancellationToken cancellationToken = default);
}