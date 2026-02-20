using BeersCheersVasis.Api.Models.LinkPreview;
using BeersCheersVasis.API.Controllers;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BeersCheersVasis.Tests.Controllers;

public class LinkPreviewControllerTests
{
    private readonly Mock<ILinkPreviewService> _mockService;
    private readonly LinkPreviewController _sut;

    public LinkPreviewControllerTests()
    {
        _mockService = new Mock<ILinkPreviewService>();
        _sut = new LinkPreviewController(_mockService.Object);
    }

    [Fact]
    public async Task GetPreviewAsync_WithUrl_ReturnsOkWithPreview()
    {
        var preview = new LinkPreviewResponse { Url = "https://example.com", Title = "Example" };
        _mockService.Setup(s => s.GetPreviewAsync("https://example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(preview);

        var result = await _sut.GetPreviewAsync("https://example.com", CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<LinkPreviewResponse>(ok.Value);
        Assert.Equal("Example", data.Title);
    }

    [Fact]
    public async Task GetPreviewAsync_NullUrl_ReturnsBadRequest()
    {
        var result = await _sut.GetPreviewAsync(null!, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetPreviewAsync_EmptyUrl_ReturnsBadRequest()
    {
        var result = await _sut.GetPreviewAsync("  ", CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetPreviewAsync_CallsServiceOnce()
    {
        _mockService.Setup(s => s.GetPreviewAsync("https://test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LinkPreviewResponse());

        await _sut.GetPreviewAsync("https://test.com", CancellationToken.None);

        _mockService.Verify(s => s.GetPreviewAsync("https://test.com", It.IsAny<CancellationToken>()), Times.Once);
    }
}
