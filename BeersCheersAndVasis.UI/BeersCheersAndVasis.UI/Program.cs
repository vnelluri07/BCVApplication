using BeersCheersAndVasis.UI.Components;
using BeersCheersAndVasis.UI.Components.Pages.Script;
using BeersCheersVasis.Api.Client;
using BeersCheersVasis.Api.Client.Implementations;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddRazorComponents()
//    .AddInteractiveWebAssemblyComponents();

//builder.Services.AddMudServices();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseWebAssemblyDebugging();
//}
//else
//{
//    app.UseExceptionHandler("/Error", createScopeForErrors: true);
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();

//app.UseStaticFiles();
//app.UseAntiforgery();

//app.MapRazorComponents<App>()
//    .AddInteractiveWebAssemblyRenderMode()
//    .AddAdditionalAssemblies(typeof(BeersCheersAndVasis.UI.Client._Imports).Assembly);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var domain = builder.Configuration["AppSettings:ApiBaseUrl"] ?? throw new ArgumentNullException("API Address");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(domain) });

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;

    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.RequireInteraction = false;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
});

//builder.Services.AddBlazoredSessionStorage(config => config.JsonSerializerOptions.WriteIndented = true);
//builder.Services.AddAuthorizationCore(options =>
//    options.AddPolicy("HsipAuthorizationPolicy", policy =>
//        policy.Requirements.Add(new HsipRoleRequirement())));
//builder.Services.AddScoped<HsipAuthenticationService>();
//builder.Services.AddScoped<AuthenticationStateProvider>(x => x.GetRequiredService<HsipAuthenticationService>());
//builder.Services.AddScoped<IAuthorizationHandler, HsipAuthorizationHandler>();

//builder.Services.AddHsipHttpClient((services, options) =>
//{
//    options.BaseAddress = new Uri(domain);
//    options.GetAuthToken = async () =>
//    {
//        string? authToken = null;
//        var auth = services.GetRequiredService<HsipAuthenticationService>();
//        var state = await auth.GetAuthenticationStateAsync();
//        if (state.User?.Identity?.IsAuthenticated ?? false)
//        {
//            var claimsIdentity = (ClaimsIdentity)state.User.Identity;
//            var accessTokenClaim = claimsIdentity.FindFirst(ClaimTypes.UserData);
//            authToken = accessTokenClaim?.Value;
//        }
//        return authToken;
//    };
//});


// API Client
try
{
	builder.Services.AddScoped<IApiClient, ApiClient>();
}
catch (Exception ex)
{

	throw;
}
//builder.Services.AddScoped<IAuthenticateApi, AuthenticateApi>();
builder.Services.AddScoped<IUserApi, UserApi>();

// Local Services
//builder.Services.AddScoped<IUserInfoService, UserInfoService>();

// View Controllers
builder.Services.AddScoped<ScriptController>();

await builder.Build().RunAsync();
