using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts.Interfaces;

namespace HomeworkAssignment.Infrastructure.Implementations.Helpers;

public partial class ProcessService : IProcessService
{
    public async Task<ProcessResult> RunProcessAsync(ProcessStartInfo startInfo, CancellationToken cancellationToken)
    {
        using var process = new Process();
        process.StartInfo = startInfo;

        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) outputBuilder.AppendLine(e.Data);
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) errorBuilder.AppendLine(e.Data);
        };

        try
        {
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync(cancellationToken);

            var cleanedOutput = CompileAnsiEscapeRegex().Replace(outputBuilder.ToString(), "");

            return new ProcessResult
            {
                ExitCode = process.ExitCode,
                ErrorDataReceived = errorBuilder.ToString(),
                OutputDataReceived = cleanedOutput
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while running the process.", ex);
        }
    }

    [GeneratedRegex(@"\x1B\[[0-?9;]*[mK]")]
    private static partial Regex CompileAnsiEscapeRegex();
}