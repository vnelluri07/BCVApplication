using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITurnstileService _turnstileService;

    public AuthController(IAuthService authService, ITurnstileService turnstileService)
    {
        _authService = authService;
        _turnstileService = turnstileService;
    }

    [HttpPost("google")]
    public async Task<IActionResult> GoogleLoginAsync([FromBody] GoogleLoginRequest request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.TurnstileToken))
        {
            var isHuman = await _turnstileService.VerifyAsync(request.TurnstileToken, cancellationToken);
            if (!isHuman) return BadRequest("Turnstile verification failed.");
        }

        var result = await _authService.GoogleLoginAsync(request.IdToken, cancellationToken);
        return Ok(result);
    }

    [HttpPost("google-code")]
    public async Task<IActionResult> GoogleLoginWithCodeAsync([FromBody] GoogleCodeRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.GoogleLoginWithCodeAsync(request.Code, request.RedirectUri, cancellationToken);
        return Ok(result);
    }

    [HttpPost("verify-turnstile")]
    public async Task<IActionResult> VerifyTurnstileAsync([FromBody] TurnstileRequest request, CancellationToken cancellationToken)
    {
        var isHuman = await _turnstileService.VerifyAsync(request.Token, cancellationToken);
        return Ok(new { success = isHuman });
    }
}

public sealed class GoogleLoginRequest
{
    public string IdToken { get; set; } = string.Empty;
    public string? TurnstileToken { get; set; }
}

public sealed class GoogleCodeRequest
{
    public string Code { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
}

public sealed class TurnstileRequest
{
    public string Token { get; set; } = string.Empty;
}
