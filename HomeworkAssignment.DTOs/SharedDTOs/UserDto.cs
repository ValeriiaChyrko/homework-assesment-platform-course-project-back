namespace HomeAssignment.DTOs.SharedDTOs;

public class UserDto
{
    public Guid Id { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public List<string> Roles { get; set; }
    public string? GithubUsername { get; init; }
    public string? GithubProfileUrl { get; init; }
    public string? GithubPictureUrl { get; init; }
}