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
                5, TimeSpan.FromSeconds(60), null));
        });

        services.AddScoped<IUnitOfWork, BcvUnitOfWork>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IScriptRepository, ScriptRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();

        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IScriptService, ScriptService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IAppUserService, AppUserService>();
        services.AddSingleton<IAnonymousNameGenerator, AnonymousNameGenerator>();

        services.AddScoped<IImageService>(sp =>
        {
            var env = sp.GetRequiredService<IWebHostEnvironment>();
            var path = Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot"), "uploads", "images");
            return new ImageService(path);
        });

        services.AddHttpClient("LinkPreview", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("BCVApp/1.0");
        });
        services.AddScoped<ILinkPreviewService, LinkPreviewService>();
    }
}
