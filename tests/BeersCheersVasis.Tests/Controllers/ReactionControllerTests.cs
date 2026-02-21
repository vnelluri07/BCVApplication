using BeersCheersVasis.Api.Models.Reaction;
using BeersCheersVasis.API.Controllers;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BeersCheersVasis.Tests.Controllers;

public class ReactionControllerTests
{
    private readonly Mock<IReactionService> _mockService = new();
    private readonly ReactionController _sut;

    public ReactionControllerTests() => _sut = new ReactionController(_mockService.Object);

    [Fact]
    public async Task React_ReturnsOkWithCounts()
    {
        var request = new ReactRequest { ScriptId = 1, VoterKey = "v", ReactionType = ReactionType.Like };
        var expected = new ReactionCountsResponse { ScriptId = 1, Likes = 1 };
        _mockService.Setup(s => s.ReactAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _sut.React(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
    }

    [Fact]
    public async Task GetScriptCounts_ReturnsOk()
    {
        var expected = new ReactionCountsResponse { ScriptId = 1, Likes = 3 };
        _mockService.Setup(s => s.GetCountsAsync(1, null, "v", It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _sut.GetScriptCounts(1, "v", CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expected, ok.Value);
    }

    [Fact]
    public async Task GetBulkScriptCounts_ReturnsOk()
    {
        var ids = new[] { 1, 2 };
        var expected = new List<ReactionCountsResponse> { new() { ScriptId = 1 }, new() { ScriptId = 2 } };
        _mockService.Setup(s => s.GetBulkCountsAsync(ids, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.GetBulkScriptCounts(ids, null, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RemoveScriptReaction_ReturnsOk()
    {
        var result = await _sut.RemoveScriptReaction(1, "v", CancellationToken.None);
        Assert.IsType<OkResult>(result);
        _mockService.Verify(s => s.RemoveReactionAsync(1, null, "v", It.IsAny<CancellationToken>()), Times.Once);
    }
}
