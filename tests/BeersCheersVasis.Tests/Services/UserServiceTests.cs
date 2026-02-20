using BeersCheersVasis.Api.Models.User;
using BeersCheersVasis.Repo;
using BeersCheersVasis.Services.Implementation;
using Moq;

namespace BeersCheersVasis.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _sut = new UserService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetUsers_ReturnsUsersFromRepository()
    {
        var expected = new List<UserResponse>
        {
            new() { Id = 1, Name = "Alice" },
            new() { Id = 2, Name = "Bob" }
        };
        _mockRepo.Setup(r => r.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var result = await _sut.GetUsers(CancellationToken.None);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetUsers_WhenEmpty_ReturnsEmptyCollection()
    {
        _mockRepo.Setup(r => r.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<UserResponse>());

        var result = await _sut.GetUsers(CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUsers_DelegatesToRepository()
    {
        _mockRepo.Setup(r => r.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<UserResponse>());

        await _sut.GetUsers(CancellationToken.None);

        _mockRepo.Verify(r => r.GetUserAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
