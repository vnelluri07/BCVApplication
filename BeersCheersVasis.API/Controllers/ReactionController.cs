using BeersCheersVasis.Api.Models.Reaction;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReactionController : ControllerBase
{
    private readonly IReactionService _reactionService;
    public ReactionController(IReactionService reactionService) => _reactionService = reactionService;

    [HttpPost]
    public async Task<IActionResult> React([FromBody] ReactRequest request, CancellationToken cancellationToken)
    {
        var result = await _reactionService.ReactAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("script/{scriptId}")]
    public async Task<IActionResult> GetScriptCounts(int scriptId, [FromQuery] string? voterKey, CancellationToken cancellationToken)
    {
        var result = await _reactionService.GetCountsAsync(scriptId, null, voterKey, cancellationToken);
        return Ok(result);
    }

    [HttpPost("script/bulk")]
    public async Task<IActionResult> GetBulkScriptCounts([FromBody] int[] scriptIds, [FromQuery] string? voterKey, CancellationToken cancellationToken)
    {
        var result = await _reactionService.GetBulkCountsAsync(scriptIds, voterKey, cancellationToken);
        return Ok(result);
    }

    [HttpPost("comment/bulk")]
    public async Task<IActionResult> GetCommentCounts([FromBody] int[] commentIds, [FromQuery] string? voterKey, CancellationToken cancellationToken)
    {
        var result = await _reactionService.GetCommentCountsAsync(commentIds, voterKey, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("script/{scriptId}")]
    public async Task<IActionResult> RemoveScriptReaction(int scriptId, [FromQuery] string voterKey, CancellationToken cancellationToken)
    {
        await _reactionService.RemoveReactionAsync(scriptId, null, voterKey, cancellationToken);
        return Ok();
    }

    [HttpDelete("comment/{commentId}")]
    public async Task<IActionResult> RemoveCommentReaction(int commentId, [FromQuery] string voterKey, CancellationToken cancellationToken)
    {
        await _reactionService.RemoveReactionAsync(null, commentId, voterKey, cancellationToken);
        return Ok();
    }
}
