using BeersCheersVasis.Api.Models.SubCategory;

namespace BeersCheersVasis.Services;

public interface ISubCategoryService
{
    Task<IEnumerable<SubCategoryResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<SubCategoryResponse>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken);
    Task<SubCategoryResponse> GetAsync(int id, CancellationToken cancellationToken);
    Task<SubCategoryResponse> CreateAsync(CreateSubCategoryRequest request, CancellationToken cancellationToken);
    Task<SubCategoryResponse> UpdateAsync(UpdateSubCategoryRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
