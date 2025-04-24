using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeworkAssignment.AuthorizationFilters;

public class TeacherOnlyAttribute : Attribute, IAuthorizationFilter
{
    private const string BearerPrefix = "Bearer ";
    private const string RealmAccessClaimType = "realm_access";
    private const string TeacherRole = "Teacher";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<TeacherOnlyAttribute>>();

        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            logger.LogWarning("Authorization header is missing.");
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authorizationHeader.ToString().Replace(BearerPrefix, string.Empty).Trim();

        if (string.IsNullOrEmpty(token))
        {
            logger.LogWarning("Authorization token is empty.");
            context.Result = new UnauthorizedResult();
            return;
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var realmAccessClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == RealmAccessClaimType)?.Value;

            if (string.IsNullOrEmpty(realmAccessClaim))
            {
                logger.LogWarning("Missing 'realm_access' claim in the token.");
                context.Result = new ForbidResult();
                return;
            }

            using var document = JsonDocument.Parse(realmAccessClaim);
            if (!document.RootElement.TryGetProperty("roles", out var rolesElement))
            {
                logger.LogWarning("'roles' not found in 'realm_access'.");
                context.Result = new ForbidResult();
                return;
            }

            var roles = rolesElement.EnumerateArray().Select(role => role.GetString()).ToList();

            if (!roles.Contains(TeacherRole))
            {
                logger.LogWarning("User does not have the 'Teacher' role.");
                context.Result = new ForbidResult();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while processing the JWT token.");
            context.Result = new UnauthorizedResult();
        }
    }
}