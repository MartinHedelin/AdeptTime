using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using parlayrunner.Web;
using parlayrunner.Shared.Interfaces;
using parlayrunner.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add authentication service
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Add saegs service
builder.Services.AddScoped<ISaegsService, SaegsService>();

// Add team selection service
builder.Services.AddSingleton<TeamSelectionService>();

// Add user role service
builder.Services.AddSingleton<UserRoleService>();

await builder.Build().RunAsync();
