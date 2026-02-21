using BeersCheersVasis.Api.Models.Reaction;
using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using BeersCheersVasis.Repository.Implementation;
using BeersCheersVasis.Tests.Helpers;
using Moq;

namespace BeersCheersVasis.Tests.Repositories;

public class ReactionRepositoryTests : IDisposable
{
    private readonly TestDbContext _dbContext;
    private readonly ReactionRepository _sut;

    public ReactionRepositoryTests()
    {
        _dbContext = TestDbContext.Create();
        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.DbContext).Returns(_dbContext);
        _sut = new ReactionRepository(mockUow.Object);
    }

    private Script SeedScript()
    {
        var script = new Script
        {
            Title = "Test", Content = "Content", IsActive = true,
            CreatedByUserId = 1, ModifiedByUserId = 1,
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        _dbContext.Script.Add(script);
        _dbContext.SaveChangesAsync(CancellationToken.None).Wait();
        return script;
    }

    private Comment SeedComment(int scriptId)
    {
        var user = new AppUser
        {
            DisplayName = "Tester", Role = "User", IsAnonymous = true,
            IsActive = true, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        _dbContext.AppUsers.Add(user);
        _dbContext.SaveChangesAsync(CancellationToken.None).Wait();

        var comment = new Comment
        {
            ScriptId = scriptId, AppUserId = user.Id, Content = "Test comment",
            CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow
        };
        _dbContext.Comments.Add(comment);
        _dbContext.SaveChangesAsync(CancellationToken.None).Wait();
        return comment;
    }

    [Fact]
    public async Task ReactAsync_CreatesNewReaction()
    {
        var script = SeedScript();
        var result = await _sut.ReactAsync(new ReactRequest
        {
            ScriptId = script.Id, VoterKey = "voter1", ReactionType = ReactionType.Like
        }, CancellationToken.None);

        Assert.Equal(1, result.Likes);
        Assert.Equal(0, result.Dislikes);
        Assert.Equal(0, result.SuperLikes);
        Assert.Equal(ReactionType.Like, result.UserReaction);
    }

    [Fact]
    public async Task ReactAsync_ToggleOff_WhenSameReactionTwice()
    {
        var script = SeedScript();
        var req = new ReactRequest { ScriptId = script.Id, VoterKey = "voter1", ReactionType = ReactionType.Like };

        await _sut.ReactAsync(req, CancellationToken.None);
        var result = await _sut.ReactAsync(req, CancellationToken.None);

        Assert.Equal(0, result.Likes);
        Assert.Null(result.UserReaction);
    }

    [Fact]
    public async Task ReactAsync_ChangesReaction_WhenDifferentType()
    {
        var script = SeedScript();
        await _sut.ReactAsync(new ReactRequest
        {
            ScriptId = script.Id, VoterKey = "voter1", ReactionType = ReactionType.Like
        }, CancellationToken.None);

        var result = await _sut.ReactAsync(new ReactRequest
        {
            ScriptId = script.Id, VoterKey = "voter1", ReactionType = ReactionType.Dislike
        }, CancellationToken.None);

        Assert.Equal(0, result.Likes);
        Assert.Equal(1, result.Dislikes);
        Assert.Equal(ReactionType.Dislike, result.UserReaction);
    }

    [Fact]
    public async Task ReactAsync_MultipleVoters_CountsCorrectly()
    {
        var script = SeedScript();
        await _sut.ReactAsync(new ReactRequest { ScriptId = script.Id, VoterKey = "a", ReactionType = ReactionType.Like }, CancellationToken.None);
        await _sut.ReactAsync(new ReactRequest { ScriptId = script.Id, VoterKey = "b", ReactionType = ReactionType.Like }, CancellationToken.None);
        await _sut.ReactAsync(new ReactRequest { ScriptId = script.Id, VoterKey = "c", ReactionType = ReactionType.SuperLike }, CancellationToken.None);

        var result = await _sut.GetCountsAsync(script.Id, null, "a", CancellationToken.None);

        Assert.Equal(2, result.Likes);
        Assert.Equal(1, result.SuperLikes);
        Assert.Equal(ReactionType.Like, result.UserReaction);
    }

    [Fact]
    public async Task GetBulkCountsAsync_ReturnsCountsForMultipleScripts()
    {
        var s1 = SeedScript();
        var s2 = SeedScript();
        await _sut.ReactAsync(new ReactRequest { ScriptId = s1.Id, VoterKey = "v", ReactionType = ReactionType.Like }, CancellationToken.None);
        await _sut.ReactAsync(new ReactRequest { ScriptId = s2.Id, VoterKey = "v", ReactionType = ReactionType.Dislike }, CancellationToken.None);

        var results = (await _sut.GetBulkCountsAsync([s1.Id, s2.Id], "v", CancellationToken.None)).ToList();

        Assert.Equal(2, results.Count);
        Assert.Equal(1, results.First(r => r.ScriptId == s1.Id).Likes);
        Assert.Equal(1, results.First(r => r.ScriptId == s2.Id).Dislikes);
    }

    [Fact]
    public async Task GetCommentCountsAsync_ReturnsCountsForComments()
    {
        var script = SeedScript();
        var comment = SeedComment(script.Id);
        await _sut.ReactAsync(new ReactRequest { CommentId = comment.Id, VoterKey = "v", ReactionType = ReactionType.Like }, CancellationToken.None);

        var results = (await _sut.GetCommentCountsAsync([comment.Id], "v", CancellationToken.None)).ToList();

        Assert.Single(results);
        Assert.Equal(1, results[0].Likes);
    }

    [Fact]
    public async Task RemoveReactionAsync_RemovesExistingReaction()
    {
        var script = SeedScript();
        await _sut.ReactAsync(new ReactRequest { ScriptId = script.Id, VoterKey = "v", ReactionType = ReactionType.Like }, CancellationToken.None);

        await _sut.RemoveReactionAsync(script.Id, null, "v", CancellationToken.None);

        var counts = await _sut.GetCountsAsync(script.Id, null, "v", CancellationToken.None);
        Assert.Equal(0, counts.Likes);
        Assert.Null(counts.UserReaction);
    }

    public void Dispose() => _dbContext.Dispose();
}
