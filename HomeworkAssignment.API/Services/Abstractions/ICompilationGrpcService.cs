using HomeAssignment.DTOs.RequestDTOs;
using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

namespace HomeworkAssignment.Services.Abstractions;

public interface ICompilationGrpcService
{
    Task<int> VerifyProjectCompilation(RequestRepositoryWithBranchDto query,
        CancellationToken cancellationToken = default);
}