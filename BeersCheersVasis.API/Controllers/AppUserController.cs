using BeersCheersVasis.Api.Models.AppUser;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "Admin")]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _appUserService.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("SetRole/{id}")]
    public async Task<IActionResult> SetRoleAsync(int id, [FromQuery] string role, CancellationToken cancellationToken)
    {
        await _appUserService.SetRoleAsync(id, role, cancellationToken);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("ToggleActive/{id}")]
    public async Task<IActionResult> ToggleActiveAsync(int id, CancellationToken cancellationToken)
    {
        await _appUserService.ToggleActiveAsync(id, cancellationToken);
        return Ok();
    }
}
