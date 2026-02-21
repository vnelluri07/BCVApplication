using BeersCheersVasis.Api.Models.Reaction;

namespace BeersCheersVasis.Api.Client.Implementations;

public sealed class ReactionApi : IReactionApi
{
    private readonly BcvHttpClient _httpClient;
    public ReactionApi(BcvHttpClient httpClient) => _httpClient = httpClient;

    public async Task<ReactionCountsResponse> ReactAsync(ReactRequest request)
        => await _httpClient.PostAsJsonAsync<ReactRequest, ReactionCountsResponse>("Reaction", request);

    public async Task<ReactionCountsResponse> GetScriptCountsAsync(int scriptId, string? voterKey)
        => await _httpClient.GetFromJsonAsync<ReactionCountsResponse>(
            $"Reaction/script/{scriptId}{(voterKey is null ? "" : $"?voterKey={Uri.EscapeDataString(voterKey)}")}");

    public async Task<IEnumerable<ReactionCountsResponse>> GetBulkScriptCountsAsync(int[] scriptIds, string? voterKey)
        => await _httpClient.PostAsJsonAsync<int[], IEnumerable<ReactionCountsResponse>>(
            $"Reaction/script/bulk{(voterKey is null ? "" : $"?voterKey={Uri.EscapeDataString(voterKey)}")}", scriptIds)
           ?? [];

    public async Task<IEnumerable<ReactionCountsResponse>> GetCommentCountsAsync(int[] commentIds, string? voterKey)
        => await _httpClient.PostAsJsonAsync<int[], IEnumerable<ReactionCountsResponse>>(
            $"Reaction/comment/bulk{(voterKey is null ? "" : $"?voterKey={Uri.EscapeDataString(voterKey)}")}", commentIds)
           ?? [];
}
