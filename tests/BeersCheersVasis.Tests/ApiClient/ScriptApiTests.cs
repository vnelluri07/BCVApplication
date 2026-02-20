using System.Net;
using System.Text.Json;
using BeersCheersVasis.Api.Client;
using BeersCheersVasis.Api.Client.Implementations;
using BeersCheersVasis.Api.Models.Script;
using Microsoft.Extensions.Options;

namespace BeersCheersVasis.Tests.ApiClient;

public class ScriptApiTests
{
    private static ScriptApi CreateSut(HttpResponseMessage response)
    {
        var handler = new MockHandler(response);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
        var options = Options.Create(new ApiClientOptions
        {
            BaseAddress = new Uri("http://test/"),
            GetAuthToken = () => Task.FromResult<string?>("fake-token")
        });
        var bcvClient = new BcvHttpClient(httpClient, options);
        return new ScriptApi(bcvClient);
    }

    private static HttpResponseMessage JsonResponse<T>(T data, HttpStatusCode status = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(status)
        {
            Content = new StringContent(
                JsonSerializer.Serialize(data),
                System.Text.Encoding.UTF8,
                "application/json")
        };
    }

    // --- ListAsync ---

    [Fact]
    public async Task ListAsync_ReturnsScripts()
    {
        var scripts = new List<ScriptResponse> { new() { Id = 1, Title = "A" } };
        var sut = CreateSut(JsonResponse(scripts));

        var result = (await sut.ListAsync()).ToList();

        Assert.Single(result);
        Assert.Equal("A", result[0].Title);
    }

    // --- CreateAsync ---

    [Fact]
    public async Task CreateAsync_ReturnsScriptResponse()
    {
        var expected = new ScriptResponse { Id = 5, Title = "Created" };
        var sut = CreateSut(JsonResponse(expected));

        var result = await sut.CreateAsync(new CreateScriptRequest { Title = "Created", Content = "C", CreatedBy = 1 });

        Assert.Equal(5, result.Id);
    }

    // --- UpdateAsync ---

    [Fact]
    public async Task UpdateAsync_ReturnsScriptResponse()
    {
        var expected = new ScriptResponse { Id = 1, Title = "Updated" };
        var sut = CreateSut(JsonResponse(expected));

        var result = await sut.UpdateAsync(new UpdateScriptRequest { Id = 1, Title = "Updated", Content = "C", ModifiedBy = 1 });

        Assert.Equal("Updated", result.Title);
    }

    [Fact]
    public async Task CreateAsync_WhenNotFound_ThrowsBcvNotFoundException()
    {
        var sut = CreateSut(new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent("") });

        await Assert.ThrowsAsync<BcvNotFoundException>(() =>
            sut.CreateAsync(new CreateScriptRequest { Title = "T", Content = "C", CreatedBy = 1 }));
    }

    [Fact]
    public async Task UpdateAsync_WhenServerError_ThrowsBcvServerErrorException()
    {
        var sut = CreateSut(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("") });

        await Assert.ThrowsAsync<BcvServerErrorException>(() =>
            sut.UpdateAsync(new UpdateScriptRequest { Id = 1, Title = "T", Content = "C", ModifiedBy = 1 }));
    }

    /// <summary>Simple HttpMessageHandler that returns a canned response.</summary>
    private class MockHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public MockHandler(HttpResponseMessage response) => _response = response;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
            => Task.FromResult(_response);
    }
}
