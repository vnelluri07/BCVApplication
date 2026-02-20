using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;
namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("AllUsers")]
    public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var result = await _userService.GetUsers(cancellationToken);
        return Ok(result);
    }
}

