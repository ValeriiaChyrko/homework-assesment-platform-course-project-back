using HomeAssignment.Domain.Abstractions.Enums;

namespace HomeAssignment.Domain.Abstractions;

public class User(
    List<Guid>? attemptsIds,
    List<Guid>? courseIds,
    List<Guid>? enrollmentIds,
    List<Guid>? userProgressIds,
    Guid id,
    string fullName,
    string email,
    string passwordHash,
    string roleType,
    string? githubUsername,
    string? githubProfileUrl,
    string? githubPictureUrl,
    DateTime createdAt,
    DateTime updatedAt)
{
    private readonly List<Guid> _attemptsIds = attemptsIds ?? [];
    private readonly List<Guid> _courseIds = courseIds ?? [];
    private readonly List<Guid> _enrollmentIds = enrollmentIds ?? [];
    private readonly List<Guid> _userProgressIds = userProgressIds ?? [];

    public Guid Id { get; init; } = id;
    public string FullName { get; set; } = fullName;
    public string Email { get; set; } = email;
    public string PasswordHash { get; set; } = passwordHash;
    public string RoleType { get; private set; } = roleType;
    public string? GithubUsername { get; set; } = githubUsername;
    public string? GithubProfileUrl { get; set; } = githubProfileUrl;
    public string? GithubPictureUrl { get; set; } = githubPictureUrl;

    public DateTime CreatedAt { get; init; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;

    public IReadOnlyList<Guid> AttemptIds => _attemptsIds.AsReadOnly();
    public IReadOnlyList<Guid> CourseIds => _courseIds.AsReadOnly();
    public IReadOnlyList<Guid> EnrollmentIds => _enrollmentIds.AsReadOnly();
    public IReadOnlyList<Guid> UsersProgressIds => _userProgressIds.AsReadOnly();

    public static User CreateStudent(string fullName, string email, string passwordHash, string? githubUsername,
        string? githubProfileUrl, string? githubPictureUrl)
    {
        var roleType = UserRoles.Student.ToString();

        var newStudent = new User(
            null,
            null,
            null,
            null,
            Guid.NewGuid(),
            fullName,
            email,
            passwordHash,
            roleType,
            githubUsername,
            githubProfileUrl,
            githubPictureUrl,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        return newStudent;
    }

    public static User CreateTeacher(string fullName, string email, string passwordHash, string? githubUsername,
        string? githubProfileUrl, string? githubPictureUrl)
    {
        var roleType = UserRoles.Teacher.ToString();

        var newTeacher = new User(
            null,
            null,
            null,
            null,
            Guid.NewGuid(),
            fullName,
            email,
            passwordHash,
            roleType,
            githubUsername,
            githubProfileUrl,
            githubPictureUrl,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        return newTeacher;
    }

    public void Update(string fullName, string email, string passwordHash, string? githubUsername,
        string? githubProfileUrl, string? githubPictureUrl)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        GithubUsername = githubUsername;
        GithubProfileUrl = githubProfileUrl;
        GithubPictureUrl = githubPictureUrl;

        UpdatedAt = DateTime.UtcNow;
    }
}