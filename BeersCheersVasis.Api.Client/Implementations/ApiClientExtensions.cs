using BeersCheersVasis.Api.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace BeersCheersVasis.Api.Client.Implementations;

public static class ApiClientExtensions
{
    public static IServiceCollection AddBcvHttpClient(this IServiceCollection services, Action<ApiClientOptions> configureOptions)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configureOptions is null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        return AddBcvHttpClient(services, (_, options) => configureOptions(options));
    }

    public static IServiceCollection AddBcvHttpClient(this IServiceCollection services, Action<IServiceProvider, ApiClientOptions> configureOptions)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configureOptions is null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        return services.AddScoped(s =>
        {
            var options = new ApiClientOptions();
            configureOptions(s, options);
            var httpClient = s.GetRequiredService<HttpClient>();
            return new BcvHttpClient(httpClient, Options.Create(options));
        });
    }
}

