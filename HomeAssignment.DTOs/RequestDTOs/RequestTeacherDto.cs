namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestTeacherDto
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string GithubUsername { get; init; }
    public required string GithubAccessToken { get; init; }
    public required string GithubProfileUrl { get; init; }
    public required string? GithubPictureUrl { get; init; }
}