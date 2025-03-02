namespace HomeAssignment.DTOs.RespondDTOs;

public class RespondUserDto
{
    public Guid Id { get; init; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? GithubUsername { get; set; }
    public string? GithubProfileUrl { get; set; }
    public string? GithubPictureUrl { get; set; }
}