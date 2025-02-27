using BeersCheersVasis.Api.Models.Script;

namespace BeersCheersVasis.Api.Client;

public interface IScriptApi
{
    public Task<IEnumerable<ScriptResponse>> ListAsync();
    public Task<string> CreateAsync(CreateScriptRequest request);
}

