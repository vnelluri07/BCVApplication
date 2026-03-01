using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using BeersCheersVasis.Repository.Implementation;
using BeersCheersVasis.Tests.Helpers;
using Moq;

namespace BeersCheersVasis.Tests.Repositories;

public class SubCategoryRepositoryTests : IDisposable
{
    private readonly TestDbContext _dbContext;
    private readonly SubCategoryRepository _sut;

    public SubCategoryRepositoryTests()
    {
        _dbContext = TestDbContext.Create();
        var mockUow = new Mock<IUnitOfWork>();
        mockUow.Setup(u => u.DbContext).Returns(_dbContext);
        _sut = new SubCategoryRepository(mockUow.Object);
    }

    private Category SeedCategory(string name = "Parent")
    {
        var cat = new Category { Name = name, IsActive = true, SortOrder = 1, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow };
        _dbContext.Categories.Add(cat);
        _dbContext.SaveChanges();
        return cat;
    }

    private SubCategory SeedSubCategory(int categoryId, string name = "Sub", bool isActive = true)
    {
        var sc = new SubCategory { CategoryId = categoryId, Name = name, IsActive = isActive, SortOrder = 1, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow };
        _dbContext.SubCategories.Add(sc);
        _dbContext.SaveChanges();
        return sc;
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAll()
    {
        var cat = SeedCategory();
        SeedSubCategory(cat.Id, "A");
        SeedSubCategory(cat.Id, "B");

        var result = (await _sut.GetAllAsync(CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByCategoryAsync_FiltersCorrectly()
    {
        var cat1 = SeedCategory("Cat1");
        var cat2 = SeedCategory("Cat2");
        SeedSubCategory(cat1.Id, "Sub1");
        SeedSubCategory(cat2.Id, "Sub2");

        var result = (await _sut.GetByCategoryAsync(cat1.Id, CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("Sub1", result[0].Name);
    }

    [Fact]
    public async Task CreateAsync_PersistsAndReturnsResponse()
    {
        var cat = SeedCategory();
        var request = new Api.Models.SubCategory.CreateSubCategoryRequest
        {
            CategoryId = cat.Id, Name = "New Sub", Description = "Desc", Icon = "🔥"
        };

        var result = await _sut.CreateAsync(request, CancellationToken.None);

        Assert.True(result.Id > 0);
        Assert.Equal("New Sub", result.Name);
        Assert.Equal(cat.Id, result.CategoryId);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task UpdateAsync_ModifiesEntity()
    {
        var cat = SeedCategory();
        var sc = SeedSubCategory(cat.Id, "Old");

        var request = new Api.Models.SubCategory.UpdateSubCategoryRequest
        {
            Id = sc.Id, CategoryId = cat.Id, Name = "Updated", IsActive = true
        };

        var result = await _sut.UpdateAsync(request, CancellationToken.None);

        Assert.Equal("Updated", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsForInvalidId()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.UpdateAsync(new Api.Models.SubCategory.UpdateSubCategoryRequest { Id = 999 }, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_SetsIsActiveFalse()
    {
        var cat = SeedCategory();
        var sc = SeedSubCategory(cat.Id, "ToDelete");

        await _sut.DeleteAsync(sc.Id, CancellationToken.None);

        var entity = _dbContext.SubCategories.Find(sc.Id)!;
        Assert.False(entity.IsActive);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsForInvalidId()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => _sut.DeleteAsync(999, CancellationToken.None));
    }

    [Fact]
    public async Task GetAllAsync_IncludesCategoryName()
    {
        var cat = SeedCategory("MyCategory");
        SeedSubCategory(cat.Id, "Sub");

        var result = (await _sut.GetAllAsync(CancellationToken.None)).First();

        Assert.Equal("MyCategory", result.CategoryName);
    }

    public void Dispose() => _dbContext.Dispose();
}
