using BeersCheersVasis.Api.Models.SubCategory;
using BeersCheersVasis.API.Controllers;
using BeersCheersVasis.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BeersCheersVasis.Tests.Controllers;

public class SubCategoryControllerTests
{
    private readonly Mock<ISubCategoryService> _mockService;
    private readonly SubCategoryController _sut;

    public SubCategoryControllerTests()
    {
        _mockService = new Mock<ISubCategoryService>();
        _sut = new SubCategoryController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithResults()
    {
        _mockService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SubCategoryResponse> { new() { Id = 1 }, new() { Id = 2 } });

        var result = await _sut.GetAllAsync(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<IEnumerable<SubCategoryResponse>>(ok.Value);
        Assert.Equal(2, data.Count());
    }

    [Fact]
    public async Task GetByCategory_ReturnsOkWithFiltered()
    {
        _mockService.Setup(s => s.GetByCategoryAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SubCategoryResponse> { new() { Id = 1, CategoryId = 5 } });

        var result = await _sut.GetByCategoryAsync(5, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Single(Assert.IsAssignableFrom<IEnumerable<SubCategoryResponse>>(ok.Value));
    }

    [Fact]
    public async Task Create_ReturnsOkWithResponse()
    {
        var request = new CreateSubCategoryRequest { CategoryId = 1, Name = "New" };
        _mockService.Setup(s => s.CreateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SubCategoryResponse { Id = 10, Name = "New" });

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(10, Assert.IsType<SubCategoryResponse>(ok.Value).Id);
    }

    [Fact]
    public async Task Update_ReturnsOkWithResponse()
    {
        var request = new UpdateSubCategoryRequest { Id = 1, CategoryId = 1, Name = "Updated" };
        _mockService.Setup(s => s.UpdateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SubCategoryResponse { Id = 1, Name = "Updated" });

        var result = await _sut.UpdateAsync(request, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Updated", Assert.IsType<SubCategoryResponse>(ok.Value).Name);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent()
    {
        _mockService.Setup(s => s.DeleteAsync(1, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _sut.DeleteAsync(1, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetAll_DelegatesToService()
    {
        _mockService.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<SubCategoryResponse>());

        await _sut.GetAllAsync(CancellationToken.None);

        _mockService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
