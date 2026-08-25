using BookStore.Client;
using BookStore.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5160") });

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<BookApiClient>();
builder.Services.AddScoped<OrderApiClient>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
