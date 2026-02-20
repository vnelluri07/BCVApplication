using BeersCheersVasis.Api.Models.User;
using BeersCheersVasis.API.Controllers;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BeersCheersVasis.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _mockService;
    private readonly UserController _sut;

    public UserControllerTests()
    {
        _mockService = new Mock<IUserService>();
        _sut = new UserController(_mockService.Object);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOkWithUsers()
    {
        var users = new List<UserResponse> { new() { Id = 1, Name = "Alice" } };
        _mockService.Setup(s => s.GetUsers(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        var result = await _sut.GetAllUsersAsync(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<IEnumerable<UserResponse>>(ok.Value);
        Assert.Single(data);
    }

    [Fact]
    public async Task GetAllUsers_WhenEmpty_ReturnsOkWithEmptyList()
    {
        _mockService.Setup(s => s.GetUsers(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<UserResponse>());

        var result = await _sut.GetAllUsersAsync(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Empty(Assert.IsAssignableFrom<IEnumerable<UserResponse>>(ok.Value));
    }

    [Fact]
    public async Task GetAllUsers_DelegatesToService()
    {
        _mockService.Setup(s => s.GetUsers(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<UserResponse>());

        await _sut.GetAllUsersAsync(CancellationToken.None);

        _mockService.Verify(s => s.GetUsers(It.IsAny<CancellationToken>()), Times.Once);
    }
}
