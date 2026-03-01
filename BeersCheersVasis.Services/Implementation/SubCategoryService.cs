using BeersCheersVasis.Api.Models.SubCategory;
using BeersCheersVasis.Repository;

namespace BeersCheersVasis.Services.Implementation;

public sealed class SubCategoryService : ISubCategoryService
{
    private readonly ISubCategoryRepository _repository;

    public SubCategoryService(ISubCategoryRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<SubCategoryResponse>> GetAllAsync(CancellationToken cancellationToken)
        => _repository.GetAllAsync(cancellationToken);

    public Task<IEnumerable<SubCategoryResponse>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken)
    {
        if (categoryId == 0) throw new ArgumentException(nameof(categoryId));
        return _repository.GetByCategoryAsync(categoryId, cancellationToken);
    }

    public Task<SubCategoryResponse> GetAsync(int id, CancellationToken cancellationToken)
    {
        if (id == 0) throw new ArgumentException(nameof(id));
        return _repository.GetAsync(id, cancellationToken);
    }

    public Task<SubCategoryResponse> CreateAsync(CreateSubCategoryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _repository.CreateAsync(request, cancellationToken);
    }

    public Task<SubCategoryResponse> UpdateAsync(UpdateSubCategoryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _repository.UpdateAsync(request, cancellationToken);
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        if (id == 0) throw new ArgumentException(nameof(id));
        return _repository.DeleteAsync(id, cancellationToken);
    }
}
