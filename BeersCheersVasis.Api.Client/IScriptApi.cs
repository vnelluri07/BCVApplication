using BeersCheersVasis.Api.Models.Script;

namespace BeersCheersVasis.Api.Client;

public interface IScriptApi
{
    public Task<IEnumerable<ScriptResponse>> ListAsync();
    public Task<ScriptResponse> CreateAsync(CreateScriptRequest request);
    public Task<ScriptResponse> UpdateAsync(UpdateScriptRequest request);
}
