using BookStore.Client;
using BookStore.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5160") });

builder.Services.AddMudServices();

// Enables Blazor client-side authorization infrastructure
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

// Register delegating handler as transient so HttpClientFactory can attach it to HTTP pipelines
builder.Services.AddTransient<JwtHandler>();

// Configure named HTTP client pointing to API base address and append JwtHandler interceptor
builder.Services.AddHttpClient("BookStoreAPI", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); // Adjust if API runs on a different port
})
.AddHttpMessageHandler<JwtHandler>();

// Override default scoped HttpClient registration so components using @inject HttpClient get the secured client
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BookStoreAPI"));

builder.Services.AddScoped<BookApiClient>();
builder.Services.AddScoped<OrderApiClient>();

await builder.Build().RunAsync();
