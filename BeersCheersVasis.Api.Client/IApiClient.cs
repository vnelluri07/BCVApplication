namespace BeersCheersVasis.Api.Client;

public interface IApiClient
{
    public IUserApi UserApi { get; }
    public IScriptApi ScriptApi { get; }
}

