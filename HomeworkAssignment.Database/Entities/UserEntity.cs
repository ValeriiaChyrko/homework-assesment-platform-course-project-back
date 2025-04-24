namespace HomeAssignment.Database.Entities;

public class UserEntity
{
    public Guid Id { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public string? GithubUsername { get; init; }
    public string? GithubProfileUrl { get; init; }
    public string? GithubPictureUrl { get; init; }

    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }

    public ICollection<UserRolesEntity>? UserRoles { get; set; }
    public ICollection<AttemptEntity>? Attempts { get; init; }
    public ICollection<CourseEntity>? Courses { get; init; }
    public ICollection<EnrollmentEntity>? Enrollments { get; init; }
    public ICollection<UserChapterProgressEntity>? UsersProgress { get; init; }
}