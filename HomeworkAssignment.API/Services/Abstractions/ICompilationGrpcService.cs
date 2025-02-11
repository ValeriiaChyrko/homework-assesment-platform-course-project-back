using HomeAssignment.DTOs.RequestDTOs;

namespace HomeworkAssignment.Services.Abstractions;

public interface ICompilationGrpcService
{
    Task<int> VerifyProjectCompilation(RequestRepositoryWithBranchDto query,
        CancellationToken cancellationToken = default);
}