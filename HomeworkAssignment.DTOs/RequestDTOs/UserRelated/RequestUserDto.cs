namespace HomeAssignment.DTOs.RequestDTOs;

public class RequestUserDto
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? GithubUsername { get; set; }
    public string? GithubProfileUrl { get; set; }
    public string? GithubPictureUrl { get; set; }
}