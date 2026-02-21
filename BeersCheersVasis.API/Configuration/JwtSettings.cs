namespace BeersCheersVasis.API.Configuration;

public sealed class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = "BeersCheersVasis.API";
    public string Audience { get; set; } = "BeersCheersVasis.App";
}
