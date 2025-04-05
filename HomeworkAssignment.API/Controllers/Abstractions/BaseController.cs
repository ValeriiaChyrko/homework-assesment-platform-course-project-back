using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Controllers.Abstractions;

public abstract class BaseController : ControllerBase
{
    protected Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? 
                          User.FindFirst("sub"); // Для Keycloak
        return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId) 
            ? userId 
            : throw new UnauthorizedAccessException("Invalid or missing user ID in token.");
    }
}
