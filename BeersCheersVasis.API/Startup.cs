using BeersCheersVasis.API.Configuration;
using BeersCheersVasis.API.Internal;
using BeersCheersVasis.Services;
using BeersCheersVasis.Services.Implementation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace BeersCheersVasis.API;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureCors();
        services.AddSingleton<IApiAuthService, ApiAuthService>();
        services.AddSingleton<IBcvApiConfigurationService, BcvApiConfigurationService>();
        services.ConfigureApi();
        services.ConfigureApiService();
        services.ConfigureSwagger();
        services.ConfigureBearerToken(_configuration);

        // Bind config sections
        var googleAuth = _configuration.GetSection("GoogleAuth").Get<GoogleAuthSettings>() ?? new();
        var turnstile = _configuration.GetSection("CloudflareTurnstile").Get<CloudflareTurnstileSettings>() ?? new();
        var jwt = _configuration.GetSection("Jwt").Get<JwtSettings>() ?? new();

        services.AddSingleton(googleAuth);
        services.AddSingleton(turnstile);
        services.AddSingleton(jwt);

        // Auth services
        services.AddHttpClient();
        services.AddScoped<ITurnstileService>(sp =>
            new TurnstileService(sp.GetRequiredService<IHttpClientFactory>(), turnstile.SecretKey));
        services.AddScoped<IAuthService>(sp =>
            new AuthService(
                sp.GetRequiredService<BeersCheersVasis.Repository.IAppUserRepository>(),
                googleAuth.ClientId, jwt.Key, jwt.Issuer, jwt.Audience,
                _configuration.GetSection("AdminEmails").Get<string[]>()));

        services.AddAuthorization();
    }

    public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseStaticFiles();
        app.UseMiddleware<Internal.RateLimitMiddleware>();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowAllCorsPolicy");
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }
}
