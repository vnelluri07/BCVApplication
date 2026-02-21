using BeersCheersVasis.Api.Models.AppUser;
using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using BeersCheersVasis.Repository.Implementation;
using BeersCheersVasis.Tests.Helpers;
using Moq;

namespace BeersCheersVasis.Tests.Repositories;

public class AppUserRepositoryTests : IDisposable
{
    private readonly TestDbContext _dbContext;
    private readonly AppUserRepository _sut;

    public AppUserRepositoryTests()
    {
        _dbContext = TestDbContext.Create();
        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.DbContext).Returns(_dbContext);
        _sut = new AppUserRepository(mockUow.Object);
    }

    [Fact]
    public async Task GetOrCreateGoogleUserAsync_CreatesNewUser()
    {
        var result = await _sut.GetOrCreateGoogleUserAsync(new GoogleAuthRequest
        {
            GoogleId = "g123", Email = "test@test.com", DisplayName = "Test User"
        }, CancellationToken.None);

        Assert.Equal("Test User", result.DisplayName);
        Assert.Equal("test@test.com", result.Email);
        Assert.Equal("User", result.Role);
        Assert.False(result.IsAnonymous);
    }

    [Fact]
    public async Task GetOrCreateGoogleUserAsync_ReturnsExisting_WhenGoogleIdMatches()
    {
        await _sut.GetOrCreateGoogleUserAsync(new GoogleAuthRequest
        {
            GoogleId = "g123", Email = "old@test.com", DisplayName = "Old Name"
        }, CancellationToken.None);

        var result = await _sut.GetOrCreateGoogleUserAsync(new GoogleAuthRequest
        {
            GoogleId = "g123", Email = "new@test.com", DisplayName = "New Name"
        }, CancellationToken.None);

        Assert.Equal("New Name", result.DisplayName);
        Assert.Equal("new@test.com", result.Email);
        Assert.Equal(1, _dbContext.AppUsers.Count());
    }

    [Fact]
    public async Task CreateAnonymousUserAsync_CreatesAnonymousUser()
    {
        var result = await _sut.CreateAnonymousUserAsync("Captain Hook #42", CancellationToken.None);

        Assert.Equal("Captain Hook #42", result.DisplayName);
        Assert.True(result.IsAnonymous);
        Assert.Equal("User", result.Role);
    }

    [Fact]
    public async Task SetRoleAsync_UpdatesRole()
    {
        var user = await _sut.GetOrCreateGoogleUserAsync(new GoogleAuthRequest
        {
            GoogleId = "g1", Email = "a@b.com", DisplayName = "A"
        }, CancellationToken.None);

        await _sut.SetRoleAsync(user.Id, "Admin", CancellationToken.None);

        var updated = await _sut.GetByIdAsync(user.Id, CancellationToken.None);
        Assert.Equal("Admin", updated!.Role);
    }

    [Fact]
    public async Task ToggleActiveAsync_TogglesIsActive()
    {
        var user = await _sut.GetOrCreateGoogleUserAsync(new GoogleAuthRequest
        {
            GoogleId = "g1", Email = "a@b.com", DisplayName = "A"
        }, CancellationToken.None);
        Assert.True(user.IsActive);

        await _sut.ToggleActiveAsync(user.Id, CancellationToken.None);
        var toggled = await _sut.GetByIdAsync(user.Id, CancellationToken.None);
        Assert.False(toggled!.IsActive);

        await _sut.ToggleActiveAsync(user.Id, CancellationToken.None);
        var toggledBack = await _sut.GetByIdAsync(user.Id, CancellationToken.None);
        Assert.True(toggledBack!.IsActive);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        await _sut.GetOrCreateGoogleUserAsync(new GoogleAuthRequest { GoogleId = "g1", Email = "a@b.com", DisplayName = "A" }, CancellationToken.None);
        await _sut.CreateAnonymousUserAsync("Anon", CancellationToken.None);

        var all = (await _sut.GetAllAsync(CancellationToken.None)).ToList();
        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task SetRoleAsync_ThrowsForNonexistentUser()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.SetRoleAsync(999, "Admin", CancellationToken.None));
    }

    public void Dispose() => _dbContext.Dispose();
}
