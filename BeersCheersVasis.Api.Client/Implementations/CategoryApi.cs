using BeersCheersVasis.Api.Models.Category;

namespace BeersCheersVasis.Api.Client.Implementations;

public sealed class CategoryApi : ICategoryApi
{
    private readonly BcvHttpClient _httpClient;

    public CategoryApi(BcvHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<CategoryResponse>> ListAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<CategoryResponse>>("Category/GetAll")
            ?? Enumerable.Empty<CategoryResponse>();
    }

    public async Task<CategoryResponse> GetAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<CategoryResponse>($"Category/Get/{id}")
            ?? new CategoryResponse();
    }

    public async Task<CategoryResponse> CreateAsync(CreateCategoryRequest request)
    {
        return await _httpClient.PostAsJsonAsync<CreateCategoryRequest, CategoryResponse>("Category/Create", request);
    }

    public async Task<CategoryResponse> UpdateAsync(UpdateCategoryRequest request)
    {
        return await _httpClient.PutAsJsonAsync<UpdateCategoryRequest, CategoryResponse>("Category/Update", request);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"Category/Delete/{id}");
    }
}
