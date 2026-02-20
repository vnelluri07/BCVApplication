namespace BeersCheersVasis.Services.Implementation;

public sealed class ImageService : IImageService
{
    private readonly string _uploadPath;

    public ImageService(string uploadPath)
    {
        _uploadPath = uploadPath;
        Directory.CreateDirectory(_uploadPath);
    }

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(fileStream, nameof(fileStream));

        var ext = Path.GetExtension(fileName)?.ToLowerInvariant() ?? ".png";
        var safeName = $"{Guid.NewGuid()}{ext}";
        var filePath = Path.Combine(_uploadPath, safeName);

        await using var fs = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fs, cancellationToken).ConfigureAwait(false);

        return $"/uploads/images/{safeName}";
    }
}
