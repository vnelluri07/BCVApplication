using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.API.Controllers;
using BeersCheersVasis.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BeersCheersVasis.Tests.Controllers;

public class ShareControllerTests
{
    private readonly Mock<IScriptRepository> _mockRepo = new();
    private readonly ShareController _sut;

    public ShareControllerTests() => _sut = new ShareController(_mockRepo.Object);

    [Fact]
    public async Task Script_ReturnsHtmlWithOgTags()
    {
        _mockRepo.Setup(r => r.GetScriptAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScriptResponse { Id = 1, Title = "Test Script", Content = "<p>Hello world</p>" });

        var result = await _sut.Script(1, CancellationToken.None);

        var content = Assert.IsType<ContentResult>(result);
        Assert.Equal("text/html", content.ContentType);
        Assert.Contains("og:title", content.Content);
        Assert.Contains("Test Script", content.Content);
        Assert.Contains("Hello world", content.Content);
        Assert.Contains("read/script/1", content.Content);
    }

    [Fact]
    public async Task Script_RedirectsOnNotFound()
    {
        _mockRepo.Setup(r => r.GetScriptAsync(999, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentException("Not found"));

        var result = await _sut.Script(999, CancellationToken.None);

        Assert.IsType<RedirectResult>(result);
    }

    [Fact]
    public async Task Script_StripsHtmlFromDescription()
    {
        _mockRepo.Setup(r => r.GetScriptAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScriptResponse { Id = 1, Title = "T", Content = "<h1>Bold</h1><p>Text here</p>" });

        var result = await _sut.Script(1, CancellationToken.None);

        var content = Assert.IsType<ContentResult>(result);
        Assert.DoesNotContain("<h1>", content.Content?.Split("og:description")[1] ?? "");
        Assert.Contains("Bold", content.Content);
    }
}
