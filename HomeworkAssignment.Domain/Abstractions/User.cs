using HomeAssignment.Domain.Abstractions.Enums;

namespace HomeAssignment.Domain.Abstractions;

public class User 
{
    private readonly List<Guid> _attemptsIds;
    private readonly List<Guid> _courseIds;
    private readonly List<Guid> _enrollmentIds;
    private readonly List<Guid> _userProgressIds;

    public User(List<Guid>? attemptsIds, List<Guid>? courseIds, List<Guid>? enrollmentIds, List<Guid>? userProgressIds, 
        Guid id, string fullName, string email, string passwordHash, string roleType, string? githubUsername, 
        string? githubProfileUrl, string? githubPictureUrl, DateTime createdAt, DateTime updatedAt)
    {
        _attemptsIds = attemptsIds ?? [];
        _courseIds = courseIds ?? [];
        _enrollmentIds = enrollmentIds ?? [];
        _userProgressIds = userProgressIds ?? [];
        Id = id;
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        RoleType = roleType;
        GithubUsername = githubUsername;
        GithubProfileUrl = githubProfileUrl;
        GithubPictureUrl = githubPictureUrl;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; init; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string RoleType { get; set; }
    public string? GithubUsername { get; set; }
    public string? GithubProfileUrl { get; set; }
    public string? GithubPictureUrl { get; set; }
    
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }
    
    public IReadOnlyList<Guid> AttemptIds => _attemptsIds.AsReadOnly();
    public IReadOnlyList<Guid> CourseIds => _courseIds.AsReadOnly();
    public IReadOnlyList<Guid> EnrollmentIds => _enrollmentIds.AsReadOnly();
    public IReadOnlyList<Guid> UsersProgressIds => _userProgressIds.AsReadOnly();

    public static User CreateStudent(List<Guid>? attemptsIds, List<Guid>? enrollmentIds, List<Guid>? userProgressIds, 
        string fullName, string email, string passwordHash, string? githubUsername, 
        string? githubProfileUrl, string? githubPictureUrl)
    {
        var roleType = UserRoles.Student.ToString();

        var newStudent = new User(
            attemptsIds,
            null,
            enrollmentIds,
            userProgressIds,
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
    
    public static User CreateTeacher(List<Guid>? attemptsIds, List<Guid>? courseIds, List<Guid>? enrollmentIds, List<Guid>? userProgressIds, 
        string fullName, string email, string passwordHash, string? githubUsername, 
        string? githubProfileUrl, string? githubPictureUrl)
    {
        var roleType = UserRoles.Teacher.ToString();

        var newTeacher = new User(
            attemptsIds,
            courseIds,
            enrollmentIds,
            userProgressIds,
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