namespace BeersCheersVasis.Services;

public interface IBcvApiConfigurationService
{
    public string BcvConnectionString { get; }

    public string AuthenticationSecretKey { get; }

    public string PortalLoginUrl { get; }
}
