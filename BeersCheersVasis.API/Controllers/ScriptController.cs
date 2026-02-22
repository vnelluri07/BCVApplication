using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.API.Controllers;

using Repository = BeersCheersVasis.Repository;

[ApiController]
[Route("[controller]")]
public class ScriptController : ControllerBase
{
    private readonly IScriptService _scriptService;
    private readonly IServiceScopeFactory _scopeFactory;

    public ScriptController(IScriptService scriptService, IServiceScopeFactory scopeFactory)
    {
        _scriptService = scriptService;
        _scopeFactory = scopeFactory;
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
        _ = Task.Run(() => BackupScriptAsync(id), CancellationToken.None);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("publish-all")]
    public async Task<IActionResult> PublishAllAsync(CancellationToken cancellationToken)
    {
        var publishedIds = await _scriptService.PublishAllScriptsAsync(cancellationToken);
        foreach (var id in publishedIds)
            _ = Task.Run(() => BackupScriptAsync(id), CancellationToken.None);
        return Ok(new { published = publishedIds.Count });
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

    [Authorize(Roles = "Admin")]
    [HttpGet("backups/{scriptId}")]
    public async Task<IActionResult> GetBackupsAsync(int scriptId, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<Repository.IScriptBackupRepository>();
        var backups = await repo.GetByScriptIdAsync(scriptId, cancellationToken);
        return Ok(backups.Select(b => new { b.Provider, b.Status, b.ExternalUrl, b.BackedUpAt, b.ErrorMessage }));
    }

    private async Task BackupScriptAsync(int scriptId)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var scriptService = scope.ServiceProvider.GetRequiredService<IScriptService>();
            var backupService = scope.ServiceProvider.GetRequiredService<IScriptBackupService>();
            var script = await scriptService.GetScriptAsync(scriptId, CancellationToken.None);
            if (script is null) return;

            var payload = new BackupPayload(script.Id, script.Title, script.Content, script.CategoryName, script.PublishedDate ?? DateTime.UtcNow);
            await backupService.BackupScriptAsync(payload, CancellationToken.None);
        }
        catch (Exception ex)
        {
            var logger = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ILogger<ScriptController>>();
            logger.LogError(ex, "Background backup failed for script {ScriptId}", scriptId);
        }
    }
}
