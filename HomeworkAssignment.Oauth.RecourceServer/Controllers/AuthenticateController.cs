using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Oauth.RecourceServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticateController : ControllerBase
{
    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = returnUrl ?? "/"
        });
    }
}