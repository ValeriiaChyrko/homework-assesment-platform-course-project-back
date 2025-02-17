using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkAssignment.Oauth.RecourceServer.Controllers;

[ApiController]
[Authorize]
[Route("resources")]
public class ResourceController: ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var user = HttpContext.User.Identity?.Name;
        return Ok(user);
    }
}