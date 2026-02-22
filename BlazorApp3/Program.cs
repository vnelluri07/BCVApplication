using BeersCheersAndVasis.UI.Components.Pages.Script;
using BeersCheersVasis.Api.Client;
using BeersCheersVasis.Api.Client.Implementations;
using BlazorApp3;
using BlazorApp3.Pages.User;
using BlazorApp3.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var domain = builder.Configuration["AppSettings:ApiBaseUrl"] ?? throw new ArgumentNullException("API Address");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(domain) });

// Auth
builder.Services.AddScoped<BcvAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<BcvAuthStateProvider>());
builder.Services.AddAuthorizationCore();

builder.Services.AddBcvHttpClient((services, options) =>
{
    var authState = services.GetRequiredService<BcvAuthStateProvider>();
    options.GetAuthToken = () => authState.GetTokenAsync();
});

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.RequireInteraction = false;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
});

// API Clients
builder.Services.AddScoped<IApiClient, ApiClient>();
builder.Services.AddScoped<IUserApi, UserApi>();
builder.Services.AddScoped<IScriptApi, ScriptApi>();
builder.Services.AddScoped<ILinkPreviewApi, LinkPreviewApi>();
builder.Services.AddScoped<ICategoryApi, CategoryApi>();
builder.Services.AddScoped<ICommentApi, CommentApi>();
builder.Services.AddScoped<IAppUserApi, AppUserApi>();
builder.Services.AddScoped<IAuthApi, AuthApi>();
builder.Services.AddScoped<IReactionApi, ReactionApi>();
builder.Services.AddScoped<ISettingsApi, SettingsApi>();

// View Controllers
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<ScriptController>();

// State
builder.Services.AddScoped<CreateScriptStateService>();

await builder.Build().RunAsync();
