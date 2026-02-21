using BeersCheersVasis.Api.Models.Category;

namespace BeersCheersVasis.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetCategoriesAsync(CancellationToken cancellationToken);
    Task<CategoryResponse> GetCategoryAsync(int id, CancellationToken cancellationToken);
    Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
    Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request, CancellationToken cancellationToken);
}
