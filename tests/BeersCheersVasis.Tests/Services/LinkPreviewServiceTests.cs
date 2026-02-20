using System.Net;
using System.Text.Json;
using BeersCheersVasis.Services.Implementation;

namespace BeersCheersVasis.Tests.Services;

public class LinkPreviewServiceTests
{
    private static LinkPreviewService CreateSut(HttpResponseMessage response)
    {
        var handler = new MockHandler(response);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
        var factory = new MockHttpClientFactory(httpClient);
        return new LinkPreviewService(factory);
    }

    // --- oEmbed ---

    [Fact]
    public async Task GetPreviewAsync_YouTubeUrl_UsesOEmbed()
    {
        var oEmbed = new { Title = "My Video", AuthorName = "Channel", ProviderName = "YouTube", ThumbnailUrl = "http://img.jpg", Html = "<iframe/>", Type = "video" };
        var sut = CreateSut(JsonResponse(oEmbed));

        var result = await sut.GetPreviewAsync("https://www.youtube.com/watch?v=abc123");

        Assert.Equal("My Video", result.Title);
        Assert.Equal("YouTube", result.SiteName);
        Assert.Equal("video", result.Type);
        Assert.Equal("<iframe/>", result.OEmbedHtml);
    }

    [Fact]
    public async Task GetPreviewAsync_VimeoUrl_UsesOEmbed()
    {
        var oEmbed = new { Title = "Vimeo Vid", ProviderName = "Vimeo", Type = "video", Html = "<iframe/>" };
        var sut = CreateSut(JsonResponse(oEmbed));

        var result = await sut.GetPreviewAsync("https://vimeo.com/12345");

        Assert.Equal("Vimeo Vid", result.Title);
        Assert.Equal("Vimeo", result.SiteName);
    }

    [Fact]
    public async Task GetPreviewAsync_TwitterUrl_UsesOEmbed()
    {
        var oEmbed = new { Title = "", AuthorName = "User", ProviderName = "Twitter", Html = "<blockquote/>" };
        var sut = CreateSut(JsonResponse(oEmbed));

        var result = await sut.GetPreviewAsync("https://twitter.com/user/status/123");

        Assert.Equal("Twitter", result.SiteName);
        Assert.Equal("<blockquote/>", result.OEmbedHtml);
    }

    // --- OpenGraph fallback ---

    [Fact]
    public async Task GetPreviewAsync_RegularUrl_FallsBackToOpenGraph()
    {
        var html = """
            <html><head>
                <meta property="og:title" content="OG Title" />
                <meta property="og:description" content="OG Desc" />
                <meta property="og:image" content="http://img.png" />
                <meta property="og:site_name" content="MySite" />
                <meta property="og:type" content="article" />
            </head><body></body></html>
            """;
        var sut = CreateSut(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(html) });

        var result = await sut.GetPreviewAsync("https://example.com/page");

        Assert.Equal("OG Title", result.Title);
        Assert.Equal("OG Desc", result.Description);
        Assert.Equal("http://img.png", result.ImageUrl);
        Assert.Equal("MySite", result.SiteName);
        Assert.Equal("article", result.Type);
    }

    [Fact]
    public async Task GetPreviewAsync_NoOgTags_FallsBackToTitleTag()
    {
        var html = "<html><head><title>Page Title</title><meta name=\"description\" content=\"Meta desc\" /></head><body></body></html>";
        var sut = CreateSut(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(html) });

        var result = await sut.GetPreviewAsync("https://example.com/page");

        Assert.Equal("Page Title", result.Title);
        Assert.Equal("Meta desc", result.Description);
    }

    // --- Error handling ---

    [Fact]
    public async Task GetPreviewAsync_OEmbedFails_FallsBackToOpenGraph()
    {
        // First call (oEmbed) returns 404, second call (OpenGraph) returns HTML
        var handler = new SequentialHandler(
            new HttpResponseMessage(HttpStatusCode.NotFound),
            new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("<html><head><title>Fallback</title></head></html>") }
        );
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://test/") };
        var sut = new LinkPreviewService(new MockHttpClientFactory(httpClient));

        var result = await sut.GetPreviewAsync("https://www.youtube.com/watch?v=fail");

        Assert.Equal("Fallback", result.Title);
    }

    [Fact]
    public async Task GetPreviewAsync_NullUrl_Throws()
    {
        var sut = CreateSut(new HttpResponseMessage(HttpStatusCode.OK));

        await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetPreviewAsync(null!));
    }

    [Fact]
    public async Task GetPreviewAsync_HttpError_ReturnsPartialResult()
    {
        var sut = CreateSut(new HttpResponseMessage(HttpStatusCode.InternalServerError));

        var result = await sut.GetPreviewAsync("https://example.com/broken");

        Assert.Equal("https://example.com/broken", result.Url);
        Assert.Equal("example.com", result.SiteName);
    }

    // --- Helpers ---

    private static HttpResponseMessage JsonResponse<T>(T data) =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), System.Text.Encoding.UTF8, "application/json")
        };

    private class MockHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client;
        public MockHttpClientFactory(HttpClient client) => _client = client;
        public HttpClient CreateClient(string name) => _client;
    }

    private class MockHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public MockHandler(HttpResponseMessage response) => _response = response;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct) => Task.FromResult(_response);
    }

    private class SequentialHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses;
        public SequentialHandler(params HttpResponseMessage[] responses) => _responses = new Queue<HttpResponseMessage>(responses);
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct) =>
            Task.FromResult(_responses.Count > 0 ? _responses.Dequeue() : new HttpResponseMessage(HttpStatusCode.OK));
    }
}
