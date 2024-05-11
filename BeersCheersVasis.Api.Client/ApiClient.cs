using BeersCheersVasis.Api.Client.Implementations;
namespace BeersCheersVasis.Api.Client;
public class ApiClient : IApiClient
{
    public ApiClient(BcvHttpClient BcvClient)
    {
        if (BcvClient is null)
        {
            throw new ArgumentNullException(nameof(BcvClient));
        }

        UserApi = new UserApi(BcvClient);
        ScriptApi = new ScriptApi(BcvClient);
    }

    //public IAuthenticateApi AuthenticateApi { get; }
    public IUserApi UserApi { get; }
    public IScriptApi ScriptApi { get; }

}

public sealed class ApiClientOptions
{
    public Uri? BaseAddress { get; set; }
    public Func<Task<string?>>? GetAuthToken { get; set; }
}

