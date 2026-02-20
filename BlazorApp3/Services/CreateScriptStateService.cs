namespace BlazorApp3.Services;

/// <summary>Holds CreateScript form state across navigation. Scoped = lives for the browser tab session.</summary>
public sealed class CreateScriptStateService
{
    public string Title { get; set; } = string.Empty;
    public string HtmlContent { get; set; } = string.Empty;
    public int? ScriptId { get; set; }
}
