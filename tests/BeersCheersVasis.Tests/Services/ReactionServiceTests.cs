using BeersCheersVasis.Api.Models.Reaction;
using BeersCheersVasis.Repository;
using BeersCheersVasis.Services.Implementation;
using Moq;

namespace BeersCheersVasis.Tests.Services;

public class ReactionServiceTests
{
    private readonly Mock<IReactionRepository> _mockRepo = new();
    private readonly ReactionService _sut;

    public ReactionServiceTests() => _sut = new ReactionService(_mockRepo.Object);

    [Fact]
    public async Task ReactAsync_DelegatesToRepository()
    {
        var request = new ReactRequest { ScriptId = 1, VoterKey = "v", ReactionType = ReactionType.Like };
        var expected = new ReactionCountsResponse { Likes = 1 };
        _mockRepo.Setup(r => r.ReactAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _sut.ReactAsync(request, CancellationToken.None);

        Assert.Equal(1, result.Likes);
        _mockRepo.Verify(r => r.ReactAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetCountsAsync_DelegatesToRepository()
    {
        var expected = new ReactionCountsResponse { SuperLikes = 5 };
        _mockRepo.Setup(r => r.GetCountsAsync(1, null, "v", It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var result = await _sut.GetCountsAsync(1, null, "v", CancellationToken.None);

        Assert.Equal(5, result.SuperLikes);
    }

    [Fact]
    public async Task RemoveReactionAsync_DelegatesToRepository()
    {
        await _sut.RemoveReactionAsync(1, null, "v", CancellationToken.None);
        _mockRepo.Verify(r => r.RemoveReactionAsync(1, null, "v", It.IsAny<CancellationToken>()), Times.Once);
    }
}
