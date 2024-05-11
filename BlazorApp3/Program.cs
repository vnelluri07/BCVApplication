using BeersCheersAndVasis.UI.Components.Pages.Script;
using BeersCheersVasis.Api.Client;
using BeersCheersVasis.Api.Client.Implementations;
using BlazorApp3;
using BlazorApp3.Pages.User;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var domain = builder.Configuration["AppSettings:ApiBaseUrl"] ?? throw new ArgumentNullException("API Address");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(domain) });

builder.Services.AddBcvHttpClient((services, options) =>
{

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

// API Client
builder.Services.AddScoped<IApiClient, ApiClient>();
//builder.Services.AddScoped<IAuthenticateApi, AuthenticateApi>();
builder.Services.AddScoped<IUserApi, UserApi>();
builder.Services.AddScoped<IScriptApi, ScriptApi>();

// Local Services
//builder.Services.AddScoped<IUserInfoService, UserInfoService>();

// View Controllers
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<ScriptController>();


await builder.Build().RunAsync();
