using BeersCheersVasis.Api.Models.Script;

namespace BeersCheersVasis.Api.Client;

public interface IScriptApi
{
    public Task<IEnumerable<ScriptResponse>> ListScriptsAsync();
}

