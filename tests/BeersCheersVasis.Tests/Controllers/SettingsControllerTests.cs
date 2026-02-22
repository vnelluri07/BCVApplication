using BeersCheersVasis.API.Controllers;
using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BeersCheersVasis.Tests.Controllers;

public class SettingsControllerTests : IDisposable
{
    private readonly TestDbContext _db;
    private readonly SettingsController _sut;

    public SettingsControllerTests()
    {
        _db = TestDbContext.Create();
        _sut = new SettingsController(_db);
    }

    [Fact]
    public async Task Get_ReturnsEmptyForMissingKey()
    {
        var result = await _sut.Get("nonexistent", CancellationToken.None);
        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value?.GetType().GetProperty("value")?.GetValue(ok.Value)?.ToString();
        Assert.Equal("", value);
    }

    [Fact]
    public async Task Set_CreatesNewSetting()
    {
        var result = await _sut.Set("greeting", new SettingRequest { Value = "Hello" }, CancellationToken.None);
        Assert.IsType<OkResult>(result);

        var setting = _db.SiteSettings.Find("greeting");
        Assert.NotNull(setting);
        Assert.Equal("Hello", setting.Value);
    }

    [Fact]
    public async Task Set_UpdatesExistingSetting()
    {
        _db.SiteSettings.Add(new SiteSetting { Key = "greeting", Value = "Old" });
        await _db.SaveChangesAsync(CancellationToken.None);

        await _sut.Set("greeting", new SettingRequest { Value = "New" }, CancellationToken.None);

        var setting = _db.SiteSettings.Find("greeting");
        Assert.Equal("New", setting!.Value);
    }

    [Fact]
    public async Task Get_ReturnsStoredValue()
    {
        _db.SiteSettings.Add(new SiteSetting { Key = "tagline", Value = "Test tagline" });
        await _db.SaveChangesAsync(CancellationToken.None);

        var result = await _sut.Get("tagline", CancellationToken.None);
        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value?.GetType().GetProperty("value")?.GetValue(ok.Value)?.ToString();
        Assert.Equal("Test tagline", value);
    }

    public void Dispose() => _db.Dispose();
}
