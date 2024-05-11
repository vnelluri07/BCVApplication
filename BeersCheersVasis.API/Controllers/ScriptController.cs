using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Services;
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

    [HttpPost("CreateScript")]
    public async Task<IActionResult> CreateScriptAsync(CreateScriptRequest request, CancellationToken cancellationToken)
    {
        //TODO: Temp stuff
        var scriptRequest = new CreateScriptRequest();
        var result = await _scriptService.CreateScriptAsync(request, cancellationToken);
        return Ok(result);
    }
}
