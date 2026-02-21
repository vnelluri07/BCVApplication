using BeersCheersVasis.Api.Models.Category;
using BeersCheersVasis.Repository;

namespace BeersCheersVasis.Services.Implementation;

public sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetCategoriesAsync(cancellationToken);
    }

    public Task<CategoryResponse> GetCategoryAsync(int id, CancellationToken cancellationToken)
    {
        if (id == 0) throw new ArgumentNullException(nameof(id));
        return _categoryRepository.GetCategoryAsync(id, cancellationToken);
    }

    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _categoryRepository.CreateCategoryAsync(request, cancellationToken);
    }

    public async Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await _categoryRepository.UpdateCategoryAsync(request, cancellationToken);
    }
}
