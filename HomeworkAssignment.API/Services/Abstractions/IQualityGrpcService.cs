using HomeAssignment.DTOs.RequestDTOs;

namespace HomeworkAssignment.Services.Abstractions;

public interface IQualityGrpcService
{
    Task<int> VerifyProjectQualityAsync(RequestRepositoryWithBranchDto query,
        CancellationToken cancellationToken = default);
}