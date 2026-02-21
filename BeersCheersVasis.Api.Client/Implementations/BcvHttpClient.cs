using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BeersCheersVasis.Api.Models;

namespace BeersCheersVasis.Api.Client.Implementations;

public class BcvForbiddenException : Exception
{
    public BcvForbiddenException()
    {
    }

    public BcvForbiddenException(string? message) : base(message)
    {
    }

    public BcvForbiddenException(string? message, Exception inner) : base(message, inner)
    {
    }
}

public class BcvNotFoundException : Exception
{
    public BcvNotFoundException()
    {
    }

    public BcvNotFoundException(string? message) : base(message)
    {
    }

    public BcvNotFoundException(string? message, Exception inner) : base(message, inner)
    {
    }
}

public class BcvServerErrorException : Exception
{
    public BcvServerErrorException()
    {
    }

    public BcvServerErrorException(string? message) : base(message)
    {
    }

    public BcvServerErrorException(string? message, Exception inner) : base(message, inner)
    {
    }
}

public class BcvConflictException : Exception
{
    public BcvConflictException()
    {
    }

    public BcvConflictException(string? message) : base(message)
    {
    }

    public BcvConflictException(string? message, Exception inner) : base(message, inner)
    {
    }
}


public class BcvHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ApiClientOptions _options;

    public BcvHttpClient(HttpClient httpClient, IOptions<ApiClientOptions> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private async Task<HttpClient> GetHttpClientAsync()
    {
        if (_options.GetAuthToken is not null)
            return _httpClient.AddAuthToken(await _options.GetAuthToken());
        return _httpClient;
    }

    public async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(string endpoint, TRequest content)
    {
        var http = await GetHttpClientAsync();
        var response = await http.PostAsJsonAsync<TRequest>(endpoint, content);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<TResponse>(_serializerOptions).ConfigureAwait(false);
            return data!;
        }
        else
        {
            var byteInfo = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var errorResponse = System.Text.Encoding.Default.GetString(byteInfo);
            var jsonMessage = string.Empty;
            if (!string.IsNullOrWhiteSpace(errorResponse))
            {
                var resMessage = errorResponse.Trim();
                if ((resMessage.StartsWith("{") && resMessage.EndsWith("}")))
                {
                    var message = JsonConvert.DeserializeObject<ErrorResponseJson>(resMessage);
                    jsonMessage = message?.message;
                }
            }
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound: throw new BcvNotFoundException(string.IsNullOrWhiteSpace(jsonMessage) ? errorResponse : jsonMessage);
                case HttpStatusCode.Forbidden: throw new BcvForbiddenException(string.IsNullOrWhiteSpace(jsonMessage) ? errorResponse : jsonMessage);
                case HttpStatusCode.InternalServerError: throw new BcvServerErrorException(string.IsNullOrWhiteSpace(jsonMessage) ? errorResponse : jsonMessage);
                case HttpStatusCode.Conflict: throw new BcvConflictException(string.IsNullOrWhiteSpace(jsonMessage) ? errorResponse : jsonMessage);
                default: throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task PostAsJsonAsync<TRequest>(string endpoint, TRequest content)
    {
        var http = await GetHttpClientAsync();
        var response = await http.PostAsJsonAsync<TRequest>(endpoint, content);
        if (response.IsSuccessStatusCode)
            return;
        throw MatchStatusCode(response.StatusCode);
    }

    public async Task<TResponse> GetFromJsonAsync<TResponse>(string endpoint)
    {
        var http = await GetHttpClientAsync();
        var response = await http.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<TResponse>(_serializerOptions).ConfigureAwait(false) ?? default(TResponse);
            return data!;
        }

        throw MatchStatusCode(response.StatusCode);
    }

    public async Task<TResponse> GetFromJsonAsync<TRequest, TResponse>(string endpoint, TRequest request = default!)
    {
        var http = await GetHttpClientAsync();
        var response = await http.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<TResponse>(_serializerOptions).ConfigureAwait(false) ?? default(TResponse);
            return data!;
        }

        throw MatchStatusCode(response.StatusCode);
    }

    public async Task<TResponse> PutAsJsonAsync<TRequest, TResponse>(string endpoint, TRequest content)
    {
        var http = await GetHttpClientAsync();
        var response = await http.PutAsJsonAsync<TRequest>(endpoint, content);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<TResponse>(_serializerOptions).ConfigureAwait(false);
            return data!;
        }

        throw MatchStatusCode(response.StatusCode);
    }

    private Exception MatchStatusCode(HttpStatusCode code)
    {
        switch (code)
        {
            case HttpStatusCode.NotFound: return new BcvNotFoundException();
            case HttpStatusCode.Forbidden: return new BcvForbiddenException();
            case HttpStatusCode.InternalServerError: return new BcvServerErrorException();
            case HttpStatusCode.Conflict: return new BcvConflictException();
            default: return new Exception(code.ToString());
        }
    }

}
