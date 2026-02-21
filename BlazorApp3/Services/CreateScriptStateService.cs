using Microsoft.JSInterop;

namespace BlazorApp3.Services;

/// <summary>Holds CreateScript form state across navigation AND browser refresh via sessionStorage.</summary>
public sealed class CreateScriptStateService
{
    private const string StorageKey = "bcv_create_script_state";
    private readonly IJSRuntime _js;

    public string Title { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public int? ScriptId { get; set; }

    public CreateScriptStateService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SaveAsync()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(new StateDto
        {
            Title = Title,
            HtmlContent = HtmlContent,
            ScriptId = ScriptId
        });
        await _js.InvokeVoidAsync("sessionStorage.setItem", StorageKey, json);
    }

    public async Task LoadAsync()
    {
        var json = await _js.InvokeAsync<string?>("sessionStorage.getItem", StorageKey);
        if (string.IsNullOrEmpty(json)) return;

        var dto = System.Text.Json.JsonSerializer.Deserialize<StateDto>(json);
        if (dto is null) return;

        Title = dto.Title;
        HtmlContent = dto.HtmlContent;
        ScriptId = dto.ScriptId;
    }

    public async Task ClearAsync()
    {
        Title = string.Empty;
        HtmlContent = string.Empty;
        ScriptId = null;
        await _js.InvokeVoidAsync("sessionStorage.removeItem", StorageKey);
    }

    private sealed class StateDto
    {
        public string Title { get; set; } = string.Empty;
        public string HtmlContent { get; set; } = string.Empty;
        public int? ScriptId { get; set; }
    }
}
