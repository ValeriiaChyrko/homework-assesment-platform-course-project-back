using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

namespace HomeworkAssignment.Services.Abstractions;

public interface IQualityGrpcService
{
    Task<int> VerifyProjectQualityAsync(RequestRepositoryWithBranchDto query,
        CancellationToken cancellationToken = default);
}