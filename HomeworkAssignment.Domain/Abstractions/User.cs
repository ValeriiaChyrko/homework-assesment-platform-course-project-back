namespace HomeAssignment.Domain.Abstractions;

public class User(
    List<int>? roleIds,
    List<Guid>? attemptsIds,
    List<Guid>? courseIds,
    List<Guid>? enrollmentIds,
    List<Guid>? userProgressIds,
    Guid id,
    string fullName,
    string email,
    string? githubUsername,
    string? githubProfileUrl,
    string? githubPictureUrl,
    DateTime createdAt,
    DateTime updatedAt)
{
    private readonly List<Guid> _attemptsIds = attemptsIds ?? [];
    private readonly List<Guid> _courseIds = courseIds ?? [];
    private readonly List<Guid> _enrollmentIds = enrollmentIds ?? [];
    private readonly List<int> _roleIds = roleIds ?? [];
    private readonly List<Guid> _userProgressIds = userProgressIds ?? [];

    public Guid Id { get; init; } = id;
    public string FullName { get; set; } = fullName;
    public string Email { get; set; } = email;

    public string? GithubUsername { get; set; } = githubUsername;
    public string? GithubProfileUrl { get; set; } = githubProfileUrl;
    public string? GithubPictureUrl { get; set; } = githubPictureUrl;

    public DateTime CreatedAt { get; init; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;

    public IReadOnlyList<int> RoleIds => _roleIds.AsReadOnly();
    public IReadOnlyList<Guid> AttemptIds => _attemptsIds.AsReadOnly();
    public IReadOnlyList<Guid> CourseIds => _courseIds.AsReadOnly();
    public IReadOnlyList<Guid> EnrollmentIds => _enrollmentIds.AsReadOnly();
    public IReadOnlyList<Guid> UsersProgressIds => _userProgressIds.AsReadOnly();

    public static User CreateUser(Guid id, string fullName, string email, string? githubUsername,
        string? githubProfileUrl, string? githubPictureUrl)
    {
        var newTeacher = new User(
            null,
            null,
            null,
            null,
            null,
            id,
            fullName,
            email,
            githubUsername,
            githubProfileUrl,
            githubPictureUrl,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        return newTeacher;
    }

    public void PatchUpdate(
        string? fullName = null,
        string? email = null,
        string? githubUsername = null,
        string? githubProfileUrl = null,
        string? githubPictureUrl = null)
    {
        FullName = fullName ?? FullName;
        Email = email ?? Email;
        GithubUsername = githubUsername ?? GithubUsername;
        GithubProfileUrl = githubProfileUrl ?? GithubProfileUrl;
        GithubPictureUrl = githubPictureUrl ?? GithubPictureUrl;

        UpdatedAt = DateTime.UtcNow;
    }
}