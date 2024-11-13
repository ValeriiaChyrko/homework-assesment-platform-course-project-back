using HomeAssignment.Domain.Abstractions.Contracts;
using HomeworkAssignment.Application.Abstractions;
using HomeworkAssignment.Infrastructure.Abstractions;

namespace HomeworkAssignment.Application.Implementations;

public class GitHubService : IGitHubService
{
    private readonly ILogger _logger;
    private readonly IStudentService _studentService;
    private readonly ITeacherService _teacherService;
    private readonly IAssignmentService _assignmentService;
    private readonly IBranchService _branchService;
    private readonly ICommitService _commitService;
    private readonly IGitHubBuildService _gitHubBuildService;

    public GitHubService(
        IStudentService studentService,
        ILogger logger,
        IAssignmentService assignmentService,
        IBranchService branchService, 
        ITeacherService teacherService,
        IGitHubBuildService gitHubBuildService, 
        ICommitService commitService) 
    {
        _studentService = studentService;
        _logger = logger;
        _assignmentService = assignmentService;
        _branchService = branchService; 
        _teacherService = teacherService;
        _gitHubBuildService = gitHubBuildService;
        _commitService = commitService;
    }

    public async Task<IEnumerable<string>?> GetStudentBranches(Guid githubProfileId, Guid assignmentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(assignmentId, cancellationToken);
            if (assignment == null) return null;

            var student = await _studentService.GetStudentByIdAsync(githubProfileId, cancellationToken);
            if (student == null) return null;

            var teacher = await _teacherService.GetTeacherByIdAsync(githubProfileId, cancellationToken);
            if (teacher == null) return null;
            
            var branches = await _branchService.GetBranchesAsync(
                teacher.GithubUsername, 
                assignment.RepositoryName, 
                cancellationToken);

            var studentBranches = await _branchService.FilterBranchesByAuthorAsync(
                teacher.GithubUsername,
                assignment.RepositoryName,
                branches.Select(b => b["name"]?.ToString())!,
                student.GithubUsername,
                cancellationToken: cancellationToken
            );

            return studentBranches;
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting GitHub branches: {ex.InnerException}.");
            throw new Exception("Error getting GitHub branches", ex);
        }
    }

    public async Task<bool> VerifyProjectCompilation(Guid githubProfileId, Guid assignmentId, string branch, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(assignmentId, cancellationToken);
            if (assignment == null) return false;

            var student = await _studentService.GetStudentByIdAsync(githubProfileId, cancellationToken);
            if (student == null) return false;

            var teacher = await _teacherService.GetTeacherByIdAsync(githubProfileId, cancellationToken);
            if (teacher == null) return false;
            
            var lastCommitSha = await _commitService.GetLastCommitByAuthorAsync(
                teacher.GithubUsername, 
                assignment.RepositoryName, 
                branch, 
                student.GithubUsername, 
                cancellationToken: cancellationToken);
            
            if (string.IsNullOrEmpty(lastCommitSha))
            {
                _logger.Log("No commits found for the specified author.");
                return false;
            }
            
            return await _gitHubBuildService.CheckIfProjectCompiles(                
                teacher.GithubUsername, 
                assignment.RepositoryName,  
                branch, 
                lastCommitSha, 
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.Log($"Error verifying project compilation: {ex.InnerException}.");
            throw new Exception("Error verifying project compilation", ex);
        }
    }
}