using BeersCheersAndVasis.UI.Data.Context;
using BeersCheersAndVasis.UI.Data.Context.Implementation;
using BeersCheersVasis.Repo;
using BeersCheersVasis.Repo.UnitOfWork;
using BeersCheersVasis.Repository;
using BeersCheersVasis.Repository.Implementation;
using BeersCheersVasis.Repository.UnitOfWork;
using BeersCheersVasis.Services;
using BeersCheersVasis.Services.Implementation;
using Microsoft.EntityFrameworkCore;

namespace BeersCheersVasis.API.Internal;

public static class ApiConfigureApiService
{
    public static void ConfigureApiService(this IServiceCollection services)
    {
        services.AddDbContext<IdbContext, dbContext>((service, options) =>
        {
            var cfgSvc = service.GetRequiredService<IBcvApiConfigurationService>();
            var env = service.GetRequiredService<IHostEnvironment>();
            options.UseSqlServer(cfgSvc.BcvConnectionString, builder => builder.EnableRetryOnFailure(
                5, TimeSpan.FromSeconds(60),null));
        });

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, BcvUnitOfWork>();
        services.AddScoped<IScriptService, ScriptService>();
        services.AddScoped<IScriptRepository, ScriptRepository>();
    }
}