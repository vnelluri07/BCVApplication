using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using BeersCheersVasis.Repository.Implementation;
using BeersCheersVasis.Tests.Helpers;
using Moq;

namespace BeersCheersVasis.Tests.Repositories;

public class ScriptRepositoryTests : IDisposable
{
    private readonly TestDbContext _dbContext;
    private readonly ScriptRepository _sut;

    public ScriptRepositoryTests()
    {
        _dbContext = TestDbContext.Create();
        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.DbContext).Returns(_dbContext);
        _sut = new ScriptRepository(mockUow.Object);
    }

    private Script SeedScript(string title = "Test", string content = "Content")
    {
        var script = new Script
        {
            Title = title,
            Content = content,
            IsActive = true,
            CreatedByUserId = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedByUserId = 1,
            ModifiedDate = DateTime.UtcNow
        };
        _dbContext.Script.Add(script);
        _dbContext.SaveChanges();
        return script;
    }

    private User SeedUser(int id = 1, string name = "TestUser")
    {
        var user = new User
        {
            Id = id,
            Name = name,
            Email = "test@test.com",
            IsActive = 1,
            PasswordSalt = new byte[] { 1 },
            PasswordHash = new byte[] { 1 },
            LastPasswordChanged = DateTime.UtcNow,
            CreatedByUserId = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedByUserId = 1,
            ModifiedDate = DateTime.UtcNow
        };
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return user;
    }

    // --- GetScriptsAsync ---

    [Fact]
    public async Task GetScriptsAsync_ReturnsAllScripts()
    {
        SeedScript("A"); SeedScript("B");

        var result = (await _sut.GetScriptsAsync(CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetScriptsAsync_WhenEmpty_ReturnsEmptyList()
    {
        var result = await _sut.GetScriptsAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetScriptsAsync_MapsFieldsCorrectly()
    {
        var script = SeedScript("Mapped", "Body");

        var result = (await _sut.GetScriptsAsync(CancellationToken.None)).First();

        Assert.Equal(script.Id, result.Id);
        Assert.Equal("Mapped", result.Title);
        Assert.Equal("Body", result.Content);
        Assert.True(result.IsActive);
    }

    // --- GetScriptAsync ---

    [Fact]
    public async Task GetScriptAsync_WithValidId_ReturnsScript()
    {
        var script = SeedScript("Find Me");

        var result = await _sut.GetScriptAsync(script.Id, CancellationToken.None);

        Assert.Equal("Find Me", result.Title);
    }

    [Fact]
    public async Task GetScriptAsync_WithInvalidId_ReturnsEmptyResponse()
    {
        var result = await _sut.GetScriptAsync(999, CancellationToken.None);

        Assert.Equal(0, result.Id);
    }

    // --- CreateScriptAsync ---

    [Fact]
    public async Task CreateScriptAsync_WithValidRequest_PersistsAndReturns()
    {
        SeedUser(1);
        var request = new CreateScriptRequest { Title = "New", Content = "Body", CreatedBy = 1 };

        var result = await _sut.CreateScriptAsync(request, CancellationToken.None);

        Assert.Equal("New", result.Title);
        Assert.True(result.Id > 0);
        Assert.Equal(1, _dbContext.Script.Count());
    }

    [Fact]
    public async Task CreateScriptAsync_WithInvalidUser_ThrowsArgumentException()
    {
        var request = new CreateScriptRequest { Title = "T", Content = "C", CreatedBy = 999 };

        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.CreateScriptAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task CreateScriptAsync_SetsIsActiveTrue()
    {
        SeedUser(1);
        var request = new CreateScriptRequest { Title = "T", Content = "C", CreatedBy = 1 };

        var result = await _sut.CreateScriptAsync(request, CancellationToken.None);

        Assert.True(result.IsActive);
    }

    // --- UpdateScriptAsync ---

    [Fact]
    public async Task UpdateScriptAsync_WithValidRequest_UpdatesFields()
    {
        var script = SeedScript("Old Title", "Old Content");
        var request = new UpdateScriptRequest
        {
            Id = script.Id,
            Title = "New Title",
            Content = "New Content",
            ModifiedBy = 2
        };

        var result = await _sut.UpdateScriptAsync(request, CancellationToken.None);

        Assert.Equal("New Title", result.Title);
        Assert.Equal("New Content", result.Content);
        Assert.Equal(2, result.ModifiedBy);
    }

    [Fact]
    public async Task UpdateScriptAsync_WithInvalidId_ThrowsArgumentException()
    {
        var request = new UpdateScriptRequest { Id = 999, Title = "T", Content = "C", ModifiedBy = 1 };

        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.UpdateScriptAsync(request, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateScriptAsync_PersistsChangesToDatabase()
    {
        var script = SeedScript("Before");
        var request = new UpdateScriptRequest { Id = script.Id, Title = "After", Content = "C", ModifiedBy = 1 };

        await _sut.UpdateScriptAsync(request, CancellationToken.None);

        var dbScript = _dbContext.Script.First(s => s.Id == script.Id);
        Assert.Equal("After", dbScript.Title);
    }

    public void Dispose() => _dbContext.Dispose();
}
