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
        try
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<ScriptResponse>>("Script/GetAllScripts");
            if (result == null)
            {
                throw new Exception("No scripts found.");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw ex.InnerException!;
        }
    }

    public async Task<ScriptResponse> CreateAsync(CreateScriptRequest request)
    {
        return await _httpClient.PostAsJsonAsync<CreateScriptRequest, ScriptResponse>("Script/create", request).ConfigureAwait(false);
    }

    public async Task<ScriptResponse> UpdateAsync(UpdateScriptRequest request)
    {
        return await _httpClient.PutAsJsonAsync<UpdateScriptRequest, ScriptResponse>("Script/update", request).ConfigureAwait(false);
    }
}
