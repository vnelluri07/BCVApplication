using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.Api.Models.Category;
using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.Repository.Implementation;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private IdbContext _dbContext => _unitOfWork.DbContext;

    public CategoryRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.SortOrder)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Icon = c.Icon,
                SortOrder = c.SortOrder,
                IsActive = c.IsActive,
                IsDeleted = c.IsDeleted,
                ScriptCount = c.Scripts.Count(s => s.IsPublished && !s.IsDeleted)
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryResponse> GetCategoryAsync(int id, CancellationToken cancellationToken)
    {
        var cat = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);
        if (cat is null) return new CategoryResponse();

        return new CategoryResponse
        {
            Id = cat.Id,
            Name = cat.Name,
            Description = cat.Description,
            Icon = cat.Icon,
            SortOrder = cat.SortOrder,
            IsActive = cat.IsActive,
            IsDeleted = cat.IsDeleted
        };
    }

    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var entity = new Category
        {
            Name = request.Name,
            Description = request.Description,
            Icon = request.Icon,
            SortOrder = request.SortOrder,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _dbContext.Categories.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CategoryResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Icon = entity.Icon,
            SortOrder = entity.SortOrder,
            IsActive = entity.IsActive
        };
    }

    public async Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
            ?? throw new ArgumentException($"Category with ID '{request.Id}' not found.");

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Icon = request.Icon;
        entity.SortOrder = request.SortOrder;
        entity.IsActive = request.IsActive;
        entity.ModifiedDate = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CategoryResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Icon = entity.Icon,
            SortOrder = entity.SortOrder,
            IsActive = entity.IsActive
        };
    }

    public async Task DeleteCategoryAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken)
            ?? throw new ArgumentException($"Category with ID '{id}' not found.");

        entity.IsDeleted = true;
        entity.ModifiedDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
