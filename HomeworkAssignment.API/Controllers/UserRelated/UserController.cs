using System.Security.Claims;
using System.Text.Json;
using HomeAssignment.DTOs.SharedDTOs;
using HomeworkAssignment.Application.Abstractions.UserRelated;
using HomeworkAssignment.Controllers.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Controllers.UserRelated;

[Produces("application/json")]
[Authorize]
[ApiController]
[Route("api/users")]
public class UserController(IUserService service)
    : BaseController
{
    private static readonly string[] AllowedRoles = ["Teacher", "Student"];

    /// <summary>
    ///     Creates a new user or updates an existing one.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var userId = GetUserId();

        var userDto = BuildUserDtoFromClaims(userId, User);
        await service.CreateOrUpdateUserAcync(userDto, cancellationToken);

        return Ok(userDto.Id);
    }

    private static UserDto BuildUserDtoFromClaims(Guid id, ClaimsPrincipal user)
    {
        var githubLogin = user.FindFirst("github_login")?.Value;

        return new UserDto
        {
            Id = id,

            FullName = GetRequiredClaim(user, "name")
                       ?? throw new InvalidOperationException("Missing full name"),

            Email = GetRequiredClaim(user, ClaimTypes.Email)
                    ?? GetRequiredClaim(user, "email")
                    ?? throw new InvalidOperationException("Missing email"),

            GithubUsername = githubLogin,
            GithubPictureUrl = user.FindFirst("avatar_url")?.Value,
            GithubProfileUrl = githubLogin is not null ? $"https://github.com/{githubLogin}" : null,
            Roles = ExtractAppRoles(user)
        };
    }

    private static List<string> ExtractAppRoles(ClaimsPrincipal user)
    {
        var realmAccess = user.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(realmAccess)) return [];

        try
        {
            var json = JsonDocument.Parse(realmAccess);
            return json.RootElement.GetProperty("roles").EnumerateArray()
                .Select(r => r.GetString())
                .Where(r => r is not null && AllowedRoles.Contains(r))
                .ToList()!;
        }
        catch
        {
            return [];
        }
    }

    private static string? GetRequiredClaim(ClaimsPrincipal user, string claimType)
    {
        return user.FindFirst(claimType)?.Value;
    }
}