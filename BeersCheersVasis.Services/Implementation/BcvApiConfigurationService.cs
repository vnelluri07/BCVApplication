using Microsoft.Extensions.Configuration;
namespace BeersCheersVasis.Services.Implementation;
public sealed class BcvApiConfigurationService : IBcvApiConfigurationService
{
    private readonly IConfiguration _configuration;

    public BcvApiConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string BcvConnectionString => _configuration.GetConnectionString("BCVConnection") ?? throw new ArgumentNullException(nameof(BcvConnectionString), "Connection string can not be null");
    //public string BcvConnectionString => _configuration["BCVConnection"] ?? throw new ArgumentNullException( nameof(BcvConnectionString), "Connection string can not be null") ;
    public string AuthenticationSecretKey { get; }
    public string PortalLoginUrl { get; }
}
