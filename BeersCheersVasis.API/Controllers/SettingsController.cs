using BeersCheersAndVasis.UI.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController : ControllerBase
{
    private readonly IdbContext _db;

    public SettingsController(IdbContext db) => _db = db;

    [HttpGet("{key}")]
    public async Task<IActionResult> Get(string key, CancellationToken cancellationToken)
    {
        var setting = await _db.SiteSettings.FindAsync([key], cancellationToken);
        return Ok(new { value = setting?.Value ?? "" });
    }

    [Authorize]
    [HttpPut("{key}")]
    public async Task<IActionResult> Set(string key, [FromBody] SettingRequest request, CancellationToken cancellationToken)
    {
        var setting = await _db.SiteSettings.FindAsync([key], cancellationToken);
        if (setting is null)
        {
            _db.SiteSettings.Add(new Data.Entities.SiteSetting { Key = key, Value = request.Value });
        }
        else
        {
            setting.Value = request.Value;
        }
        await _db.SaveChangesAsync(cancellationToken);
        return Ok();
    }
}

public sealed class SettingRequest
{
    public string Value { get; set; } = "";
}
