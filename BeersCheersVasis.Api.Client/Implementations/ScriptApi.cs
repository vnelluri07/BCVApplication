using BeersCheersVasis.Api.Models.Script;
using BeersCheersVasis.Api.Models.User;

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

    public async Task<string> CreateAsync(CreateScriptRequest request)
    {
        try
        {
            var result = await _httpClient.PostAsJsonAsync<CreateScriptRequest, string>("Script/create", request);
            if (result == null)
            {
                throw new Exception("script not saved.");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw ex.InnerException!;
        }
    }
}
