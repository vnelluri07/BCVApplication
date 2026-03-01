using System.Text.Json;
using BeersCheersVasis.Api.Client;
using BeersCheersVasis.Api.Models.Script;
using Microsoft.JSInterop;

namespace BeersCheersVasis.UI.Services;

public sealed class OfflineSyncService : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private readonly IScriptApi _scriptApi;
    private DotNetObjectReference<OfflineSyncService>? _selfRef;
    private Func<Task>? _onOnline;

    public OfflineSyncService(IJSRuntime js, IScriptApi scriptApi)
    {
        _js = js;
        _scriptApi = scriptApi;
    }

    public async Task<bool> IsOnlineAsync() =>
        await _js.InvokeAsync<bool>("bcvOffline.isOnline");

    public async Task StartMonitoringAsync(Func<Task> onOnline)
    {
        _onOnline = onOnline;
        _selfRef = DotNetObjectReference.Create(this);
        await _js.InvokeVoidAsync("bcvOffline.registerOnlineListener", _selfRef, nameof(OnBrowserOnline));
    }

    [JSInvokable]
    public async Task OnBrowserOnline()
    {
        if (_onOnline is not null)
            await _onOnline();
    }

    public async Task EnqueueDraftAsync(OfflineDraft draft)
    {
        var drafts = await LoadQueueAsync();
        var existing = drafts.FindIndex(d => d.LocalId == draft.LocalId);
        if (existing >= 0)
            drafts[existing] = draft;
        else
            drafts.Add(draft);
        await SaveQueueAsync(drafts);
    }

    public async Task<List<OfflineDraft>> LoadQueueAsync()
    {
        var json = await _js.InvokeAsync<string?>("bcvOffline.loadDrafts");
        if (string.IsNullOrEmpty(json)) return [];
        return JsonSerializer.Deserialize<List<OfflineDraft>>(json) ?? [];
    }

    public async Task<SyncResult> SyncAllAsync()
    {
        var drafts = await LoadQueueAsync();
        if (drafts.Count == 0) return new SyncResult(0, null);

        int synced = 0;
        int? lastScriptId = null;

        foreach (var draft in drafts)
        {
            try
            {
                if (draft.ScriptId.HasValue)
                {
                    var resp = await _scriptApi.UpdateAsync(new UpdateScriptRequest
                    {
                        Id = draft.ScriptId.Value,
                        Title = draft.Title,
                        Content = draft.Content,
                        ModifiedBy = draft.UserId
                    });
                    lastScriptId = resp.Id;
                }
                else
                {
                    var resp = await _scriptApi.CreateAsync(new CreateScriptRequest
                    {
                        Title = draft.Title,
                        Content = draft.Content,
                        CreatedBy = draft.UserId
                    });
                    lastScriptId = resp.Id;
                }
                synced++;
            }
            catch { break; } // still offline — stop trying
        }

        if (synced == drafts.Count)
            await _js.InvokeVoidAsync("bcvOffline.clearDrafts");
        else if (synced > 0)
            await SaveQueueAsync(drafts.Skip(synced).ToList());

        return new SyncResult(synced, lastScriptId);
    }

    private async Task SaveQueueAsync(List<OfflineDraft> drafts) =>
        await _js.InvokeVoidAsync("bcvOffline.saveDrafts", JsonSerializer.Serialize(drafts));

    public async ValueTask DisposeAsync()
    {
        _selfRef?.Dispose();
    }
}

public record OfflineDraft
{
    public string LocalId { get; init; } = Guid.NewGuid().ToString("N");
    public int? ScriptId { get; init; }
    public string Title { get; init; } = "";
    public string Content { get; init; } = "";
    public int UserId { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}

public record SyncResult(int SyncedCount, int? LastScriptId);
