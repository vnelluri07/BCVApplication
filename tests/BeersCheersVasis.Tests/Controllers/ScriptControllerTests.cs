using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.API.Controllers;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BeersCheersVasis.Tests.Controllers;

public class ScriptControllerTests
{
    private readonly Mock<IScriptService> _mockService;
    private readonly ScriptController _sut;

    public ScriptControllerTests()
    {
        _mockService = new Mock<IScriptService>();
        _sut = new ScriptController(_mockService.Object);
    }

    // --- GetAllScripts ---

    [Fact]
    public async Task GetAllScripts_ReturnsOkWithScripts()
    {
        var scripts = new List<ScriptResponse> { new() { Id = 1 }, new() { Id = 2 } };
        _mockService.Setup(s => s.GetScriptsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(scripts);

        var result = await _sut.GetAllScritsAsync(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<IEnumerable<ScriptResponse>>(ok.Value);
        Assert.Equal(2, data.Count());
    }

    [Fact]
    public async Task GetAllScripts_WhenEmpty_ReturnsOkWithEmptyList()
    {
        _mockService.Setup(s => s.GetScriptsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ScriptResponse>());

        var result = await _sut.GetAllScritsAsync(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Empty(Assert.IsAssignableFrom<IEnumerable<ScriptResponse>>(ok.Value));
    }

    // --- GetScript ---

    [Fact]
    public async Task GetScript_WithValidId_ReturnsOkWithScript()
    {
        _mockService.Setup(s => s.GetScriptAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScriptResponse { Id = 1, Title = "Found" });

        var result = await _sut.GetScriptAsync(1, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<ScriptResponse>(ok.Value);
        Assert.Equal("Found", data.Title);
    }

    // --- Create ---

    [Fact]
    public async Task Create_WithValidRequest_ReturnsOkWithResponse()
    {
        var request = new CreateScriptRequest { Title = "New", Content = "Body", CreatedBy = 1 };
        _mockService.Setup(s => s.CreateScriptAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScriptResponse { Id = 5, Title = "New" });

        var result = await _sut.CreateScriptAsync(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<ScriptResponse>(ok.Value);
        Assert.Equal(5, data.Id);
    }

    // --- Update ---

    [Fact]
    public async Task Update_WithValidRequest_ReturnsOkWithResponse()
    {
        var request = new UpdateScriptRequest { Id = 1, Title = "Updated", Content = "C", ModifiedBy = 1 };
        _mockService.Setup(s => s.UpdateScriptAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScriptResponse { Id = 1, Title = "Updated" });

        var result = await _sut.UpdateScriptAsync(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<ScriptResponse>(ok.Value);
        Assert.Equal("Updated", data.Title);
    }

    [Fact]
    public async Task GetAllScripts_DelegatesToService()
    {
        _mockService.Setup(s => s.GetScriptsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ScriptResponse>());

        await _sut.GetAllScritsAsync(CancellationToken.None);

        _mockService.Verify(s => s.GetScriptsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_DelegatesToService()
    {
        var request = new CreateScriptRequest { Title = "T", Content = "C", CreatedBy = 1 };
        _mockService.Setup(s => s.CreateScriptAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScriptResponse());

        await _sut.CreateScriptAsync(request, CancellationToken.None);

        _mockService.Verify(s => s.CreateScriptAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_DelegatesToService()
    {
        var request = new UpdateScriptRequest { Id = 1, Title = "T", Content = "C", ModifiedBy = 1 };
        _mockService.Setup(s => s.UpdateScriptAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScriptResponse());

        await _sut.UpdateScriptAsync(request, CancellationToken.None);

        _mockService.Verify(s => s.UpdateScriptAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }
}
