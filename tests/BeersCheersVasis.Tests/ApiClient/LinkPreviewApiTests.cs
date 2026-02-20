using System.Net;
using System.Text.Json;
using BeersCheersVasis.Api.Client;
using BeersCheersVasis.Api.Client.Implementations;
using BeersCheersVasis.Api.Models.LinkPreview;
using Microsoft.Extensions.Options;

namespace BeersCheersVasis.Tests.ApiClient;

public class LinkPreviewApiTests
{
    private static LinkPreviewApi CreateSut(HttpResponseMessage response)
    {
        var handler = new MockHandler(response);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
        var options = Options.Create(new ApiClientOptions
        {
            BaseAddress = new Uri("http://test/"),
            GetAuthToken = () => Task.FromResult<string?>("fake-token")
        });
        var bcvClient = new BcvHttpClient(httpClient, options);
        return new LinkPreviewApi(bcvClient);
    }

    [Fact]
    public async Task GetPreviewAsync_ReturnsPreview()
    {
        var expected = new LinkPreviewResponse { Url = "https://example.com", Title = "Example", SiteName = "example.com" };
        var response = JsonResponse(expected);

        var sut = CreateSut(response);
        var result = await sut.GetPreviewAsync("https://example.com");

        Assert.Equal("Example", result.Title);
        Assert.Equal("example.com", result.SiteName);
    }

    [Fact]
    public async Task GetPreviewAsync_EncodesUrl()
    {
        var handler = new CapturingHandler(JsonResponse(new LinkPreviewResponse()));
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
        var options = Options.Create(new ApiClientOptions
        {
            BaseAddress = new Uri("http://test/"),
            GetAuthToken = () => Task.FromResult<string?>("fake-token")
        });
        var sut = new LinkPreviewApi(new BcvHttpClient(httpClient, options));

        await sut.GetPreviewAsync("https://example.com/page");

        Assert.Contains("LinkPreview/preview?url=", handler.LastRequestUri!);
        Assert.Contains("example.com", handler.LastRequestUri!);
    }

    [Fact]
    public async Task GetPreviewAsync_NullResponse_ReturnsDefault()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
        };
        var sut = CreateSut(response);

        var result = await sut.GetPreviewAsync("https://example.com");

        Assert.Equal("https://example.com", result.Url);
    }

    private static HttpResponseMessage JsonResponse<T>(T data) =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(data), System.Text.Encoding.UTF8, "application/json")
        };

    private class MockHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public MockHandler(HttpResponseMessage response) => _response = response;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct) => Task.FromResult(_response);
    }

    private class CapturingHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public string? LastRequestUri { get; private set; }
        public CapturingHandler(HttpResponseMessage response) => _response = response;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            LastRequestUri = request.RequestUri?.ToString();
            return Task.FromResult(_response);
        }
    }
}
