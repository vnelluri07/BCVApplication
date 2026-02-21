namespace BeersCheersVasis.API.Configuration;

public sealed class CloudflareTurnstileSettings
{
    public string SiteKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}
