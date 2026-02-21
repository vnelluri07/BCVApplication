using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ScriptController : ControllerBase
{
    private readonly IScriptService _scriptService;

    public ScriptController(IScriptService scriptService)
    {
        _scriptService = scriptService;
    }

    [HttpGet("GetAllScripts")]
    public async Task<IActionResult> GetAllScritsAsync(CancellationToken cancellationToken)
    {
        var result = await _scriptService.GetScriptsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("GetPublished")]
    public async Task<IActionResult> GetPublishedAsync(CancellationToken cancellationToken)
    {
        var result = await _scriptService.GetPublishedScriptsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("GetPublished/{categoryId}")]
    public async Task<IActionResult> GetPublishedByCategoryAsync(int categoryId, CancellationToken cancellationToken)
    {
        var result = await _scriptService.GetPublishedByCategoryAsync(categoryId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("GetScript/{id}")]
    public async Task<IActionResult> GetScriptAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _scriptService.GetScriptAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateScriptAsync(CreateScriptRequest request, CancellationToken cancellationToken)
    {
        var result = await _scriptService.CreateScriptAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateScriptAsync(UpdateScriptRequest request, CancellationToken cancellationToken)
    {
        var result = await _scriptService.UpdateScriptAsync(request, cancellationToken);
        return Ok(result);
    }

    // Admin endpoints
    [Authorize(Roles = "Admin")]
    [HttpPut("publish/{id}")]
    public async Task<IActionResult> PublishAsync(int id, CancellationToken cancellationToken)
    {
        await _scriptService.PublishScriptAsync(id, cancellationToken);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("publish-all")]
    public async Task<IActionResult> PublishAllAsync(CancellationToken cancellationToken)
    {
        var count = await _scriptService.PublishAllScriptsAsync(cancellationToken);
        return Ok(new { published = count });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("unpublish/{id}")]
    public async Task<IActionResult> UnpublishAsync(int id, CancellationToken cancellationToken)
    {
        await _scriptService.UnpublishScriptAsync(id, cancellationToken);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("soft-delete/{id}")]
    public async Task<IActionResult> SoftDeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _scriptService.SoftDeleteScriptAsync(id, cancellationToken);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("restore/{id}")]
    public async Task<IActionResult> RestoreAsync(int id, CancellationToken cancellationToken)
    {
        await _scriptService.RestoreScriptAsync(id, cancellationToken);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("set-category/{id}/{categoryId}")]
    public async Task<IActionResult> SetCategoryAsync(int id, int categoryId, CancellationToken cancellationToken)
    {
        await _scriptService.SetCategoryAsync(id, categoryId, cancellationToken);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("schedule/{id}")]
    public async Task<IActionResult> ScheduleAsync(int id, [FromQuery] DateTime publishDate, CancellationToken cancellationToken)
    {
        await _scriptService.ScheduleScriptAsync(id, publishDate, cancellationToken);
        return Ok();
    }
}
