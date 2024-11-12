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
    private readonly IGitHubClient _gitHubClient;

    public GitHubService(IStudentService studentService, ILogger logger, IAssignmentService assignmentService, IGitHubClient gitHubClient, ITeacherService teacherService)
    {
        _studentService = studentService;
        _logger = logger;
        _assignmentService = assignmentService;
        _gitHubClient = gitHubClient;
        _teacherService = teacherService;
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

            var branches = await _gitHubClient.GetBranches(student.GithubUsername, "test-conection", cancellationToken);
            var studentBranches = await _gitHubClient.FilterBranchesByAuthor(
                    teacher.GithubUsername, 
                    "test-conection", 
                    branches, 
                    student.GithubUsername, 
                    cancellationToken
                );
            return studentBranches;
        }
        catch (Exception ex)
        {
            _logger.Log($"Error getting GitHub branches: {ex.InnerException}.");
            throw new Exception("Error getting GitHub branches", ex);
        }
    }
}