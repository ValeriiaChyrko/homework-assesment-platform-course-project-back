using HomeworkAssignment.Infrastructure.Abstractions;
using HomeworkAssignment.Infrastructure.Abstractions.Contracts;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

namespace HomeworkAssignment.Infrastructure.Implementations;

public class CodeQualityService : ICodeQualityService
{
    public async Task<int> CheckCodeQualityAsync(string repoDirectory, CancellationToken cancellationToken = default)
    {
        if (!MSBuildLocator.IsRegistered)
        {
            MSBuildLocator.RegisterDefaults();
        }
    
        var workspace = MSBuildWorkspace.Create(new Dictionary<string, string>
        {
            { "AlwaysCompileMarkupFilesInSeparateDomain", "true" },
            { "Configuration", "Debug" },
            { "Platform", "AnyCPU" }
        });
    
        var projects = Directory.GetFiles(repoDirectory, "*.csproj", SearchOption.AllDirectories);
        var diagnosticsList = new List<DiagnosticMessage>();

        foreach (var projectFile in projects)
        {
            var project = await workspace.OpenProjectAsync(projectFile, cancellationToken: cancellationToken);
            var compilation = await project.GetCompilationAsync(cancellationToken);

            if (compilation == null) continue;

            var diagnostics = compilation.GetDiagnostics(cancellationToken);

            diagnosticsList.AddRange(from diagnostic in diagnostics 
                let severity = diagnostic.Severity.ToString() 
                let message = diagnostic.ToString() 
                select new DiagnosticMessage
                {
                    Message = message, 
                    Severity = severity
                });
        }

        var score = EvaluateCodeQuality(diagnosticsList);
        return score; 
    }
    
    private static int EvaluateCodeQuality(IEnumerable<DiagnosticMessage>? diagnostics)
    {
        var diagnosticMessages = diagnostics?.ToList();
        if (diagnostics == null || !diagnosticMessages!.Any())
        {
            return 100;
        }

        var errorCount = diagnosticMessages!.Count(d => d.Severity == "Error");
        var warningCount = diagnosticMessages!.Count(d => d.Severity == "Warning");
        var infoCount = diagnosticMessages!.Count(d => d.Severity == "Info");
        
        const int errorWeight = 25; 
        const int warningWeight = 15; 
        const int infoWeight = 5; 
        
        var score = 100 - (errorCount * errorWeight + warningCount * warningWeight + infoCount * infoWeight);
        return Math.Clamp(score, 0, 100);
    }
}