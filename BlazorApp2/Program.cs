using BlazorApp2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var domain = builder.Configuration["AppSettings:ApiBaseUrl"] ?? throw new ArgumentNullException("API Address");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(domain) });

await builder.Build().RunAsync();
