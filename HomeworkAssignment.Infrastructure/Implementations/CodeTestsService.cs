using System.Diagnostics;
using System.Text.RegularExpressions;
using HomeworkAssignment.Infrastructure.Abstractions;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;

namespace HomeworkAssignment.Infrastructure.Implementations;

public class CodeTestsService : ICodeTestsService
{
    public async Task<int> CheckCodeTestsAsync(string testsDirectory, CancellationToken cancellationToken = default)
    {
        const string failedPattern = @"Failed \w+ \[(.*?)\s+ms\]";
        const string passedPattern = @"Passed \w+ \[(.*?)\s+ms\]";
        if (!Directory.Exists(testsDirectory))
        {
            return 100;
        }
       
        var testFiles = Directory.GetFiles(testsDirectory, "*.csproj", SearchOption.AllDirectories);
        var diagnosticMessages = new List<DiagnosticMessage>();

        foreach (var testFile in testFiles)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"test \"{testFile}\" --verbosity normal",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.OutputDataReceived += (_, args) =>
            {
                if (string.IsNullOrEmpty(args.Data)) return;
                
                if (Regex.IsMatch(args.Data, failedPattern))
                {
                    diagnosticMessages.Add(new DiagnosticMessage
                    {
                        Message = args.Data,
                        Severity = "Error"
                    });
                }
                else if (Regex.IsMatch(args.Data, passedPattern))
                {
                    diagnosticMessages.Add(new DiagnosticMessage
                    {
                        Message = args.Data,
                        Severity = "Info"
                    });
                }
            };
            
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync(cancellationToken);
        }

        var score = EvaluateCodeTestsPassed(diagnosticMessages);
        return score;
    }
    
    private static int EvaluateCodeTestsPassed(IEnumerable<DiagnosticMessage>? diagnostics)
    {
        var diagnosticMessages = diagnostics?.ToList();
        if (diagnostics == null || !diagnosticMessages!.Any())
        {
            return 100;
        }

        var failedCount = diagnosticMessages!.Count(d => d.Severity == "Error");
        var passedCount = diagnosticMessages!.Count(d => d.Severity == "Info");
       
        var totalCount = failedCount + passedCount; 
        var score = (passedCount / (double)totalCount) * 100;

        return (int)Math.Clamp(score, 0, 100); 
    }
}
