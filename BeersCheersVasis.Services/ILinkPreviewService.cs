using BeersCheersVasis.Api.Models.LinkPreview;

namespace BeersCheersVasis.Services;

public interface ILinkPreviewService
{
    Task<LinkPreviewResponse> GetPreviewAsync(string url, CancellationToken cancellationToken = default);
}
