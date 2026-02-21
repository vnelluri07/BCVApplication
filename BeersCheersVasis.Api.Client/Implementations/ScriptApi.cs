using BeersCheersVasis.Api.Models.Script;

namespace BeersCheersVasis.Api.Client.Implementations;

public sealed class ScriptApi : IScriptApi
{
    private readonly BcvHttpClient _httpClient;

    public ScriptApi(BcvHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<ScriptResponse>> ListAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ScriptResponse>>("Script/GetAllScripts")
            ?? Enumerable.Empty<ScriptResponse>();
    }

    public async Task<IEnumerable<ScriptResponse>> GetPublishedAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ScriptResponse>>("Script/GetPublished")
            ?? Enumerable.Empty<ScriptResponse>();
    }

    public async Task<IEnumerable<ScriptResponse>> GetPublishedAsync(int categoryId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<ScriptResponse>>($"Script/GetPublished/{categoryId}")
            ?? Enumerable.Empty<ScriptResponse>();
    }

    public async Task<ScriptResponse> GetAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ScriptResponse>($"Script/GetScript/{id}")
            ?? new ScriptResponse();
    }

    public async Task<ScriptResponse> CreateAsync(CreateScriptRequest request)
    {
        return await _httpClient.PostAsJsonAsync<CreateScriptRequest, ScriptResponse>("Script/create", request);
    }

    public async Task<ScriptResponse> UpdateAsync(UpdateScriptRequest request)
    {
        return await _httpClient.PutAsJsonAsync<UpdateScriptRequest, ScriptResponse>("Script/update", request);
    }

    public async Task PublishAsync(int id) => await _httpClient.PutAsync($"Script/publish/{id}");
    public async Task UnpublishAsync(int id) => await _httpClient.PutAsync($"Script/unpublish/{id}");
    public async Task SoftDeleteAsync(int id) => await _httpClient.DeleteAsync($"Script/soft-delete/{id}");
    public async Task SetCategoryAsync(int id, int categoryId) => await _httpClient.PutAsync($"Script/set-category/{id}/{categoryId}");
}
