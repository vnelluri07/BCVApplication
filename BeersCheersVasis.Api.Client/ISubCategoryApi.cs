using BeersCheersVasis.Api.Models.SubCategory;

namespace BeersCheersVasis.Api.Client;

public interface ISubCategoryApi
{
    Task<IEnumerable<SubCategoryResponse>> ListAsync();
    Task<IEnumerable<SubCategoryResponse>> ListByCategoryAsync(int categoryId);
    Task<SubCategoryResponse> GetAsync(int id);
    Task<SubCategoryResponse> CreateAsync(CreateSubCategoryRequest request);
    Task<SubCategoryResponse> UpdateAsync(UpdateSubCategoryRequest request);
    Task DeleteAsync(int id);
}
