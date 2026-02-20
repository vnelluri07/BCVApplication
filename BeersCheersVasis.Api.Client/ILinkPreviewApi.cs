using BeersCheersVasis.Api.Models.LinkPreview;

namespace BeersCheersVasis.Api.Client;

public interface ILinkPreviewApi
{
    Task<LinkPreviewResponse> GetPreviewAsync(string url);
}
