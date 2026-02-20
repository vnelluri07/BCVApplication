using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class LinkPreviewController : ControllerBase
{
    private readonly ILinkPreviewService _linkPreviewService;

    public LinkPreviewController(ILinkPreviewService linkPreviewService)
    {
        _linkPreviewService = linkPreviewService;
    }

    [HttpGet("preview")]
    public async Task<IActionResult> GetPreviewAsync([FromQuery] string url, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest("URL is required");

        var result = await _linkPreviewService.GetPreviewAsync(url, cancellationToken);
        return Ok(result);
    }
}
