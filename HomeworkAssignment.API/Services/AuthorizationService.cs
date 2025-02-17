using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Primitives;
using OpenIddict.Abstractions;

namespace HomeworkAssignment.Services;

public class AuthorizationService
{
    public IDictionary<string, StringValues> ParseOAuthParameters(HttpContext httpContext, HashSet<string>? excluding = null)
    {
        var parameters = httpContext.Request.HasFormContentType
            ? httpContext.Request.Form
                .Where(v => excluding != null && !excluding.Contains(v.Key))
                .ToDictionary(v => v.Key, v => v.Value)
            : httpContext.Request.Query
                .Where(v => excluding != null && !excluding.Contains(v.Key))
                .ToDictionary(v => v.Key, v => v.Value);

        return parameters;
    }

    public static string BuildRedirectUrl(HttpRequest request, IDictionary<string, StringValues> oAuthParameters)
    {
        var uriBuilder = new UriBuilder
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port ?? (request.IsHttps ? 443 : 80),
            Path = request.PathBase + request.Path,
            Query = QueryString.Create(oAuthParameters).ToString()
        };
        return uriBuilder.ToString();
    }

    public static bool IsAuthenticated(AuthenticateResult authenticateResult, OpenIddictRequest request)
    {
        if (!authenticateResult.Succeeded)
        {
            return false;
        }

        if (!request.MaxAge.HasValue || authenticateResult.Properties == null) return true;
        
        var maxAgeSeconds = TimeSpan.FromSeconds(request.MaxAge.Value);
        var expired = !authenticateResult.Properties.IssuedUtc.HasValue ||
                      DateTimeOffset.UtcNow - authenticateResult.Properties.IssuedUtc > maxAgeSeconds;
        return !expired;

    }

    public static HashSet<string> GetDestinations(ClaimsIdentity identity, Claim claim)
    {
        var destinations = new HashSet<string>();

        if (claim.Type is not (OpenIddictConstants.Claims.Name or OpenIddictConstants.Claims.Email))
            return destinations;
        destinations.Add(OpenIddictConstants.Destinations.AccessToken);

        if (identity.HasScope(OpenIddictConstants.Scopes.OpenId))
        {
            destinations.Add(OpenIddictConstants.Destinations.IdentityToken);
        }

        return destinations;
    }
}