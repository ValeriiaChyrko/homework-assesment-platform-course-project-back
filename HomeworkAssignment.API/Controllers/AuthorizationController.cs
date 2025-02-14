using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace HomeworkAssignment.Controllers;

[ApiController]
[Route("connect")]
public class AuthorizationController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AuthorizationController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("token"), Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        if (request == null)
        {
            return BadRequest(new { error = "Invalid request" });
        }

        if (request.IsPasswordGrantType())
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized(new { error = "Invalid username or password" });
            }

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return Unauthorized(new { error = "Invalid username or password" });
            }

            var claims = new List<Claim>
            {
                new(Claims.Subject, user.Id),
                new(Claims.Name, user.UserName),
                new(Claims.Email, user.Email),
                new(Claims.Role, "User") // Додати ролі, якщо потрібно
            };

            var identity = new ClaimsIdentity(claims, TokenValidationParameters.DefaultAuthenticationType, Claims.Name, Claims.Role);
            var principal = new ClaimsPrincipal(identity);
            principal.SetScopes(Scopes.OpenId, Scopes.Email, Scopes.Profile);

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return BadRequest(new { error = "Unsupported grant type" });
    }

    [Authorize, HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Logged out successfully" });
    }
    
    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("userinfo")]
    public async Task<IActionResult> UserInfo()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            Name = User.Identity?.Name,
            Email = User.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Email)?.Value,
            Role = User.Claims.FirstOrDefault(c => c.Type == OpenIddictConstants.Claims.Role)?.Value
        });
    }
}