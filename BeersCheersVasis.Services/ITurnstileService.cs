namespace BeersCheersVasis.Services;

public interface ITurnstileService
{
    Task<bool> VerifyAsync(string token, CancellationToken cancellationToken);
}
