using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Pratice.Controllers;

public abstract class BaseController : Controller
{
    protected string UserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User not authenticated");
}
