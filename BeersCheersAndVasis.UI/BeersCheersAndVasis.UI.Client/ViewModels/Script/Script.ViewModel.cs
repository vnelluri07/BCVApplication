namespace BeersCheersAndVasis.UI.ViewModels.Script;

public record ScriptViewModel
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Content { get; init; }
    public bool IsActive { get; init; }
    public bool IsSelected { get; init; }

    public ScriptViewModel()
    {
        Id = 0;
        Title = string.Empty;
        Content = string.Empty;
        IsActive = true;
        IsSelected = false;
    }

    public ScriptViewModel(int? id, string? title, string? content, bool isActive)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(title, nameof(title));
        ArgumentNullException.ThrowIfNull(content, nameof(content));

        Id = id ?? 1221;
        Title = title!;
        Content = content!;
        IsActive = isActive;
    }
}
