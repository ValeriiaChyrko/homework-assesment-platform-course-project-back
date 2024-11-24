using System.Diagnostics;

namespace HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;

public interface IProcessService
{
    Task<ProcessResult> RunProcessAsync(ProcessStartInfo startInfo, CancellationToken cancellationToken);
}