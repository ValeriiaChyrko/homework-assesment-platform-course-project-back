namespace HomeAssignment.Database.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string RoleType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<GitHubProfilesEntity>? GitHubProfiles { get; set; }
}