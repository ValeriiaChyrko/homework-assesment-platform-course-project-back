using HomeAssignment.DTOs.RequestDTOs;

namespace HomeworkAssignment.Services.Abstractions;

public interface IAccountGrpcService
{
    Task<IReadOnlyList<string>?>
        GetBranchesAsync(RequestBranchDto query, CancellationToken cancellationToken = default);
}