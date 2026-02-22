using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using BeersCheersVasis.Repository.Implementation;
using BeersCheersVasis.Tests.Helpers;
using Moq;

namespace BeersCheersVasis.Tests.Repositories;

public class CategoryRepositoryTests : IDisposable
{
    private readonly TestDbContext _dbContext;
    private readonly CategoryRepository _sut;

    public CategoryRepositoryTests()
    {
        _dbContext = TestDbContext.Create();
        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.DbContext).Returns(_dbContext);
        _sut = new CategoryRepository(mockUow.Object);
    }

    private Category SeedCategory(string name = "Test", bool isActive = true)
    {
        var cat = new Category
        {
            Name = name,
            IsActive = isActive,
            SortOrder = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _dbContext.Categories.Add(cat);
        _dbContext.SaveChanges();
        return cat;
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsAllIncludingInactive()
    {
        SeedCategory("Active", isActive: true);
        SeedCategory("Inactive", isActive: false);

        var result = (await _sut.GetCategoriesAsync(CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task DeleteCategoryAsync_SetsIsActiveFalse()
    {
        var cat = SeedCategory("ToDelete", isActive: true);

        await _sut.DeleteCategoryAsync(cat.Id, CancellationToken.None);

        var entity = _dbContext.Categories.Find(cat.Id)!;
        Assert.False(entity.IsActive);
    }

    [Fact]
    public async Task DeleteCategoryAsync_ThrowsForInvalidId()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.DeleteCategoryAsync(999, CancellationToken.None));
    }

    public void Dispose() => _dbContext.Dispose();
}
