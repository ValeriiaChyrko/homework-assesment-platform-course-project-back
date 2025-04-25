using HomeAssignment.DTOs.RequestDTOs.AttemptRelated;

namespace HomeworkAssignment.Services.Abstractions;

public interface IAccountGrpcService
{
    Task<IReadOnlyList<string>?>
        GetBranchesAsync(RequestBranchDto query, CancellationToken cancellationToken = default);
    
    Task<string>PostBranchAsync(RequestBranchDto query, CancellationToken cancellationToken = default);
}