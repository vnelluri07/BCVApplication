using BeersCheersVasis.Api.Models.Comment;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _commentService.GetAllCommentsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("GetByScript/{scriptId}")]
    public async Task<IActionResult> GetByScriptAsync(int scriptId, CancellationToken cancellationToken)
    {
        var result = await _commentService.GetCommentsByScriptAsync(scriptId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync(CreateCommentRequest request, CancellationToken cancellationToken)
    {
        var result = await _commentService.CreateCommentAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _commentService.DeleteCommentAsync(id, cancellationToken);
        return Ok();
    }

    [HttpGet("Search/{scriptId}")]
    public async Task<IActionResult> SearchAsync(int scriptId, [FromQuery] string keyword, CancellationToken cancellationToken)
    {
        var result = await _commentService.SearchCommentsAsync(scriptId, keyword, cancellationToken);
        return Ok(result);
    }
}
