namespace BeersCheersVasis.Api.Models.LinkPreview;

public sealed class LinkPreviewResponse
{
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
    public string Type { get; set; } = "website"; // website, video, article
    public string? OEmbedHtml { get; set; } // For YouTube/Twitter rich embeds
}
