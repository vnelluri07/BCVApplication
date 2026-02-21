namespace BeersCheersVasis.Api.Models.Comment;

public sealed class CreateCommentRequest
{
    public int ScriptId { get; set; }
    public int? ParentCommentId { get; set; }
    public string Content { get; set; }
    public int? AppUserId { get; set; }
    public string? AnonymousDisplayName { get; set; }
}
