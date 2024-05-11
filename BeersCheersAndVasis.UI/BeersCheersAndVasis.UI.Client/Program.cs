using BeersCheersVasis.Api.Client;
using BeersCheersVasis.Api.Client.Implementations;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

await builder.Build().RunAsync();
