using BeersCheersVasis.Api.Models.AppUser;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AppUserController : ControllerBase
{
    private readonly IAppUserService _appUserService;

    public AppUserController(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }

    [HttpPost("GoogleAuth")]
    public async Task<IActionResult> GoogleAuthAsync(GoogleAuthRequest request, CancellationToken cancellationToken)
    {
        var result = await _appUserService.GetOrCreateGoogleUserAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("Anonymous")]
    public async Task<IActionResult> CreateAnonymousAsync(CancellationToken cancellationToken)
    {
        var result = await _appUserService.GetOrCreateAnonymousUserAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _appUserService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
