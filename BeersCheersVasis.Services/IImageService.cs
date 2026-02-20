namespace BeersCheersVasis.Services;

public interface IImageService
{
    Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
}
