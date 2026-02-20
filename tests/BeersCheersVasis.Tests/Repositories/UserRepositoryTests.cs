using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using BeersCheersVasis.Repository.Implementation;
using BeersCheersVasis.Tests.Helpers;
using Moq;

namespace BeersCheersVasis.Tests.Repositories;

public class UserRepositoryTests : IDisposable
{
    private readonly TestDbContext _dbContext;
    private readonly UserRepository _sut;

    public UserRepositoryTests()
    {
        _dbContext = TestDbContext.Create();
        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.DbContext).Returns(_dbContext);
        _sut = new UserRepository(mockUow.Object);
    }

    private void SeedUser(string name = "Alice", string email = "alice@test.com")
    {
        _dbContext.Users.Add(new User
        {
            Name = name,
            Email = email,
            IsActive = 1,
            PasswordSalt = new byte[] { 1 },
            PasswordHash = new byte[] { 1 },
            LastPasswordChanged = DateTime.UtcNow,
            CreatedByUserId = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedByUserId = 1,
            ModifiedDate = DateTime.UtcNow
        });
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetUserAsync_ReturnsAllUsers()
    {
        SeedUser("Alice"); SeedUser("Bob", "bob@test.com");

        var result = (await _sut.GetUserAsync(CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetUserAsync_WhenEmpty_ReturnsEmptyCollection()
    {
        var result = await _sut.GetUserAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserAsync_MapsFieldsCorrectly()
    {
        SeedUser("Charlie", "charlie@test.com");

        var result = (await _sut.GetUserAsync(CancellationToken.None)).First();

        Assert.Equal("Charlie", result.Name);
        Assert.Equal("charlie@test.com", result.Email);
        Assert.Equal(1, result.IsActive);
    }

    public void Dispose() => _dbContext.Dispose();
}
