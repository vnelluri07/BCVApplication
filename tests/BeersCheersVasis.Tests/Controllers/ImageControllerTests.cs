using BeersCheersVasis.API.Controllers;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BeersCheersVasis.Tests.Controllers;

public class ImageControllerTests
{
    private readonly Mock<IImageService> _mockService;
    private readonly ImageController _sut;

    public ImageControllerTests()
    {
        _mockService = new Mock<IImageService>();
        _sut = new ImageController(_mockService.Object);
    }

    [Fact]
    public async Task UploadAsync_WithFile_ReturnsOkWithLocation()
    {
        var file = CreateMockFile("test.jpg", new byte[] { 1, 2, 3 });
        _mockService.Setup(s => s.UploadImageAsync(It.IsAny<Stream>(), "test.jpg", It.IsAny<CancellationToken>()))
            .ReturnsAsync("/uploads/images/abc.jpg");

        var result = await _sut.UploadAsync(file, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var location = ok.Value!.GetType().GetProperty("location")!.GetValue(ok.Value) as string;
        Assert.Equal("/uploads/images/abc.jpg", location);
    }

    [Fact]
    public async Task UploadAsync_NullFile_ReturnsBadRequest()
    {
        var result = await _sut.UploadAsync(null!, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UploadAsync_EmptyFile_ReturnsBadRequest()
    {
        var file = CreateMockFile("empty.jpg", Array.Empty<byte>());

        var result = await _sut.UploadAsync(file, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UploadAsync_CallsServiceWithCorrectFileName()
    {
        var file = CreateMockFile("photo.png", new byte[] { 1 });
        _mockService.Setup(s => s.UploadImageAsync(It.IsAny<Stream>(), "photo.png", It.IsAny<CancellationToken>()))
            .ReturnsAsync("/uploads/images/x.png");

        await _sut.UploadAsync(file, CancellationToken.None);

        _mockService.Verify(s => s.UploadImageAsync(It.IsAny<Stream>(), "photo.png", It.IsAny<CancellationToken>()), Times.Once);
    }

    private static IFormFile CreateMockFile(string name, byte[] content)
    {
        var mock = new Mock<IFormFile>();
        mock.Setup(f => f.FileName).Returns(name);
        mock.Setup(f => f.Length).Returns(content.Length);
        mock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(content));
        return mock.Object;
    }
}
