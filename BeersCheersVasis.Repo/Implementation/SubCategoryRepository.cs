using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersVasis.Api.Models.SubCategory;
using BeersCheersVasis.Data.Entities;
using BeersCheersVasis.Repo.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.Repository.Implementation;

public sealed class SubCategoryRepository : ISubCategoryRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private IdbContext _dbContext => _unitOfWork.DbContext;

    public SubCategoryRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SubCategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SubCategories
            .Include(sc => sc.Category)
            .OrderBy(sc => sc.Category.SortOrder).ThenBy(sc => sc.SortOrder)
            .Select(sc => MapToResponse(sc))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SubCategoryResponse>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken)
    {
        return await _dbContext.SubCategories
            .Include(sc => sc.Category)
            .Where(sc => sc.CategoryId == categoryId)
            .OrderBy(sc => sc.SortOrder)
            .Select(sc => MapToResponse(sc))
            .ToListAsync(cancellationToken);
    }

    public async Task<SubCategoryResponse> GetAsync(int id, CancellationToken cancellationToken)
    {
        var sc = await _dbContext.SubCategories.Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        return sc is null ? new SubCategoryResponse() : MapToResponse(sc);
    }

    public async Task<SubCategoryResponse> CreateAsync(CreateSubCategoryRequest request, CancellationToken cancellationToken)
    {
        var entity = new SubCategory
        {
            CategoryId = request.CategoryId,
            Name = request.Name,
            Description = request.Description,
            Icon = request.Icon,
            SortOrder = request.SortOrder,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _dbContext.SubCategories.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Reload with Category nav
        return await GetAsync(entity.Id, cancellationToken);
    }

    public async Task<SubCategoryResponse> UpdateAsync(UpdateSubCategoryRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Id == request.Id, cancellationToken)
            ?? throw new ArgumentException($"SubCategory with ID '{request.Id}' not found.");

        entity.CategoryId = request.CategoryId;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Icon = request.Icon;
        entity.SortOrder = request.SortOrder;
        entity.IsActive = request.IsActive;
        entity.ModifiedDate = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return await GetAsync(entity.Id, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.SubCategories.FirstOrDefaultAsync(sc => sc.Id == id, cancellationToken)
            ?? throw new ArgumentException($"SubCategory with ID '{id}' not found.");

        entity.IsActive = false;
        entity.ModifiedDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static SubCategoryResponse MapToResponse(SubCategory sc) => new()
    {
        Id = sc.Id,
        CategoryId = sc.CategoryId,
        CategoryName = sc.Category?.Name,
        Name = sc.Name,
        Description = sc.Description,
        Icon = sc.Icon,
        SortOrder = sc.SortOrder,
        IsActive = sc.IsActive,
        ScriptCount = sc.Scripts?.Count(s => s.IsPublished && !s.IsDeleted) ?? 0
    };
}
