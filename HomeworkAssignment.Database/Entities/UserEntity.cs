namespace HomeAssignment.Database.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string RoleType { get; set; }
    public string? GithubUsername { get; set; }
    public string? GithubProfileUrl { get; set; }
    public string? GithubPictureUrl { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<AttemptProgressEntity>? Attempts { get; set; } 
    public ICollection<CourseEntity>? Courses { get; set; } 
    public ICollection<EnrollmentEntity>? Enrollments { get; set; } 
    public ICollection<UserProgressEntity>? UsersProgress { get; set; } 
}