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
        //test
        //var connectionString = _configuration.GetConnectionString("BcvDBConnection");
        //services.AddDbContext<dbContext>(options => options.UseSqlServer(connectionString));
        services.ConfigureApi(); 
        services.ConfigureApiService();
        services.ConfigureSwagger();
        services.ConfigureBearerToken(_configuration);
    }

    public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseRouting();
        //app.UseAuthentication();
        //app.UseAuthorization();
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
