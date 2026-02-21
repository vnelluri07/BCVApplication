using System.Net.Http.Headers;
namespace BeersCheersVasis.Api.Client.Implementations;


public static class HttpClientExtensions
{
    public static HttpClient AddAuthToken(this HttpClient client, string? token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}

