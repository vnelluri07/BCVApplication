using BeersCheersVasis.Api.Models.SubCategory;

namespace BeersCheersVasis.Api.Client.Implementations;

public sealed class SubCategoryApi : ISubCategoryApi
{
    private readonly BcvHttpClient _httpClient;

    public SubCategoryApi(BcvHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<SubCategoryResponse>> ListAsync()
        => await _httpClient.GetFromJsonAsync<IEnumerable<SubCategoryResponse>>("SubCategory/GetAll")
            ?? Enumerable.Empty<SubCategoryResponse>();

    public async Task<IEnumerable<SubCategoryResponse>> ListByCategoryAsync(int categoryId)
        => await _httpClient.GetFromJsonAsync<IEnumerable<SubCategoryResponse>>($"SubCategory/GetByCategory/{categoryId}")
            ?? Enumerable.Empty<SubCategoryResponse>();

    public async Task<SubCategoryResponse> GetAsync(int id)
        => await _httpClient.GetFromJsonAsync<SubCategoryResponse>($"SubCategory/Get/{id}")
            ?? new SubCategoryResponse();

    public async Task<SubCategoryResponse> CreateAsync(CreateSubCategoryRequest request)
        => await _httpClient.PostAsJsonAsync<CreateSubCategoryRequest, SubCategoryResponse>("SubCategory/Create", request);

    public async Task<SubCategoryResponse> UpdateAsync(UpdateSubCategoryRequest request)
        => await _httpClient.PutAsJsonAsync<UpdateSubCategoryRequest, SubCategoryResponse>("SubCategory/Update", request);

    public async Task DeleteAsync(int id)
        => await _httpClient.DeleteAsync($"SubCategory/Delete/{id}");
}
