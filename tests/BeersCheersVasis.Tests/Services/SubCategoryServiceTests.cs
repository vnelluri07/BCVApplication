using BeersCheersVasis.Api.Models.SubCategory;
using BeersCheersVasis.Repository;
using BeersCheersVasis.Services.Implementation;
using Moq;

namespace BeersCheersVasis.Tests.Services;

public class SubCategoryServiceTests
{
    private readonly Mock<ISubCategoryRepository> _mockRepo;
    private readonly SubCategoryService _sut;

    public SubCategoryServiceTests()
    {
        _mockRepo = new Mock<ISubCategoryRepository>();
        _sut = new SubCategoryService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllAsync_DelegatesToRepository()
    {
        _mockRepo.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SubCategoryResponse> { new() { Id = 1 } });

        var result = (await _sut.GetAllAsync(CancellationToken.None)).ToList();

        Assert.Single(result);
        _mockRepo.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_ThrowsForZeroId()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.GetAsync(0, CancellationToken.None));
    }

    [Fact]
    public async Task GetByCategoryAsync_ThrowsForZeroId()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.GetByCategoryAsync(0, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_ThrowsForNullRequest()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.CreateAsync(null!, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsForNullRequest()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _sut.UpdateAsync(null!, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_ThrowsForZeroId()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.DeleteAsync(0, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_DelegatesToRepository()
    {
        var request = new CreateSubCategoryRequest { CategoryId = 1, Name = "Test" };
        _mockRepo.Setup(r => r.CreateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SubCategoryResponse { Id = 1, Name = "Test" });

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        Assert.Equal("Test", result.Name);
        _mockRepo.Verify(r => r.CreateAsync(request, It.IsAny<CancellationToken>()), Times.Once);
    }
}
