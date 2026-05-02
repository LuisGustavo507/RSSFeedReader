using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RSSFeedReader.UI;
using RSSFeedReader.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var backendBaseUrl = builder.Configuration["BackendBaseUrl"]
    ?? throw new InvalidOperationException("BackendBaseUrl not configured in wwwroot/appsettings.json");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(backendBaseUrl) });
builder.Services.AddScoped<SubscriptionApiClient>();

await builder.Build().RunAsync();
