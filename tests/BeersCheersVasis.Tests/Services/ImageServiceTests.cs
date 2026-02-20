using BeersCheersVasis.Services.Implementation;

namespace BeersCheersVasis.Tests.Services;

public class ImageServiceTests : IDisposable
{
    private readonly string _tempDir;
    private readonly ImageService _sut;

    public ImageServiceTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"img_test_{Guid.NewGuid()}");
        _sut = new ImageService(_tempDir);
    }

    public void Dispose() => Directory.Delete(_tempDir, true);

    [Fact]
    public async Task UploadImageAsync_SavesFileAndReturnsUrl()
    {
        var content = new byte[] { 1, 2, 3 };
        using var stream = new MemoryStream(content);

        var url = await _sut.UploadImageAsync(stream, "photo.jpg", CancellationToken.None);

        Assert.StartsWith("/uploads/images/", url);
        Assert.EndsWith(".jpg", url);
        var savedFile = Directory.GetFiles(_tempDir).Single();
        Assert.Equal(content, await File.ReadAllBytesAsync(savedFile));
    }

    [Fact]
    public async Task UploadImageAsync_GeneratesUniqueNames()
    {
        using var s1 = new MemoryStream(new byte[] { 1 });
        using var s2 = new MemoryStream(new byte[] { 2 });

        var url1 = await _sut.UploadImageAsync(s1, "a.png", CancellationToken.None);
        var url2 = await _sut.UploadImageAsync(s2, "a.png", CancellationToken.None);

        Assert.NotEqual(url1, url2);
        Assert.Equal(2, Directory.GetFiles(_tempDir).Length);
    }

    [Fact]
    public async Task UploadImageAsync_PreservesExtension()
    {
        using var stream = new MemoryStream(new byte[] { 1 });

        var url = await _sut.UploadImageAsync(stream, "image.webp", CancellationToken.None);

        Assert.EndsWith(".webp", url);
    }

    [Fact]
    public async Task UploadImageAsync_NullStream_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.UploadImageAsync(null!, "test.png", CancellationToken.None));
    }

    [Fact]
    public void Constructor_CreatesDirectory()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"img_ctor_{Guid.NewGuid()}");
        try
        {
            _ = new ImageService(dir);
            Assert.True(Directory.Exists(dir));
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }
}
