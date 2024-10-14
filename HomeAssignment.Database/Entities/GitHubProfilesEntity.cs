namespace HomeAssignment.Database.Entities;

public sealed class GitHubProfilesEntity
{
    public Guid Id { get; set; }
    
    public required string GithubUsername { get; set; }
    public required string GithubAccessToken { get; set; }
    public required string GithubProfileUrl { get; set; }
    public string? GithubPictureUrl { get; set; }
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public ICollection<AssignmentEntity>? Assignments { get; set; }
    public ICollection<AttemptEntity>? Attempts { get; set; }
}