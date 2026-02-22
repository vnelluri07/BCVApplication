using BeersCheersVasis.Api.Models.Category;

namespace BeersCheersVasis.Api.Client;

public interface ICategoryApi
{
    Task<IEnumerable<CategoryResponse>> ListAsync();
    Task<CategoryResponse> GetAsync(int id);
    Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);
    Task<CategoryResponse> UpdateAsync(UpdateCategoryRequest request);
    Task DeleteAsync(int id);
}
