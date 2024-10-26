namespace HomeAssignment.DTOs.SharedDTOs;

public class UserDto
{
    public Guid Id { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string PasswordHash { get; init; }
    public required string RoleType { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}