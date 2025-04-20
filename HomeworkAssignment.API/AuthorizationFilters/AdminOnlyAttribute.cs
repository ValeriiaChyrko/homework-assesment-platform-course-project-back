using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeworkAssignment.AuthorizationFilters;

public class AdminOnlyAttribute : Attribute, IAuthorizationFilter
{
    private const string BearerPrefix = "Bearer ";
    private const string GroupsClaimType = "groups";
    private const string AdminGroup = "Admins";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<AdminOnlyAttribute>>();

        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            logger.LogWarning("Authorization header is missing.");
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authorizationHeader.ToString().Replace(BearerPrefix, string.Empty);

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
            var groups = jwtToken.Claims
                .Where(c => c.Type == GroupsClaimType)
                .Select(c => c.Value);

            if (!groups.Contains(AdminGroup))
            {
                logger.LogWarning("User  is not in the 'Admins' group.");
                context.Result = new ForbidResult();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the authorization token.");
            context.Result = new UnauthorizedResult();
        }
    }
}