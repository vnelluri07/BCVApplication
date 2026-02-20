using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Repository;
using BeersCheersVasis.Services.Implementation;
using Moq;

namespace BeersCheersVasis.Tests.Services;

public class ScriptServiceTests
{
    private readonly Mock<IScriptRepository> _mockRepo;
    private readonly ScriptService _sut;

    public ScriptServiceTests()
    {
        _mockRepo = new Mock<IScriptRepository>();
        _sut = new ScriptService(_mockRepo.Object);
    }

    // --- GetScriptsAsync ---

    [Fact]
    public async Task GetScriptsAsync_ReturnsScriptsFromRepository()
    {
        var expected = new List<ScriptResponse>
        {
            new() { Id = 1, Title = "Script 1" },
            new() { Id = 2, Title = "Script 2" }
        };
        _mockRepo.Setup(r => r.GetScriptsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.GetScriptsAsync(CancellationToken.None);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetScriptsAsync_WhenEmpty_ReturnsEmptyCollection()
    {
        _mockRepo.Setup(r => r.GetScriptsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ScriptResponse>());

        var result = await _sut.GetScriptsAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    // --- GetScriptAsync ---

    [Fact]
    public async Task GetScriptAsync_WithValidId_ReturnsScript()
    {
        _mockRepo.Setup(r => r.GetScriptAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScriptResponse { Id = 1, Title = "Test" });

        var result = await _sut.GetScriptAsync(1, CancellationToken.None);

        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetScriptAsync_WithZeroId_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.GetScriptAsync(0, CancellationToken.None));
    }

    // --- CreateScriptAsync ---

    [Fact]
    public async Task CreateScriptAsync_WithValidRequest_ReturnsResponse()
    {
        var request = new CreateScriptRequest { Title = "New", Content = "Body", CreatedBy = 1 };
        var expected = new ScriptResponse { Id = 10, Title = "New", Content = "Body" };
        _mockRepo.Setup(r => r.CreateScriptAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.CreateScriptAsync(request, CancellationToken.None);

        Assert.Equal(10, result.Id);
    }

    [Fact]
    public async Task CreateScriptAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.CreateScriptAsync(null!, CancellationToken.None));
    }

    // --- UpdateScriptAsync ---

    [Fact]
    public async Task UpdateScriptAsync_WithValidRequest_ReturnsResponse()
    {
        var request = new UpdateScriptRequest { Id = 1, Title = "Updated", Content = "New body", ModifiedBy = 1 };
        var expected = new ScriptResponse { Id = 1, Title = "Updated", Content = "New body" };
        _mockRepo.Setup(r => r.UpdateScriptAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.UpdateScriptAsync(request, CancellationToken.None);

        Assert.Equal("Updated", result.Title);
    }

    [Fact]
    public async Task UpdateScriptAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.UpdateScriptAsync(null!, CancellationToken.None));
    }

    [Fact]
    public async Task GetScriptsAsync_DelegatesToRepository()
    {
        _mockRepo.Setup(r => r.GetScriptsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<ScriptResponse>());

        await _sut.GetScriptsAsync(CancellationToken.None);

        _mockRepo.Verify(r => r.GetScriptsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateScriptAsync_DelegatesToRepository()
    {
        var request = new CreateScriptRequest { Title = "T", Content = "C", CreatedBy = 1 };
        _mockRepo.Setup(r => r.CreateScriptAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ScriptResponse());

        await _sut.CreateScriptAsync(request, CancellationToken.None);

        _mockRepo.Verify(r => r.CreateScriptAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }
}
