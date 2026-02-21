using BeersCheersVasis.Api.Models.Comment;

namespace BeersCheersVasis.Api.Client.Implementations;

public sealed class CommentApi : ICommentApi
{
    private readonly BcvHttpClient _httpClient;

    public CommentApi(BcvHttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IEnumerable<CommentResponse>> GetByScriptAsync(int scriptId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<CommentResponse>>($"Comment/GetByScript/{scriptId}")
            ?? Enumerable.Empty<CommentResponse>();
    }

    public async Task<CommentResponse> CreateAsync(CreateCommentRequest request)
    {
        return await _httpClient.PostAsJsonAsync<CreateCommentRequest, CommentResponse>("Comment/Create", request);
    }

    public async Task DeleteAsync(int id)
    {
        await _httpClient.DeleteAsync($"Comment/Delete/{id}");
    }

    public async Task<IEnumerable<CommentResponse>> SearchAsync(int scriptId, string keyword)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<CommentResponse>>($"Comment/Search/{scriptId}?keyword={Uri.EscapeDataString(keyword)}")
            ?? Enumerable.Empty<CommentResponse>();
    }
}
