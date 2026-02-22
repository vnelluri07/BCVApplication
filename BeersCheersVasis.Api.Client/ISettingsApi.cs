namespace BeersCheersVasis.Api.Client;

public interface ISettingsApi
{
    Task<string> GetAsync(string key);
    Task SetAsync(string key, string value);
}
