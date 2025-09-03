using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using parlayrunner.Web;
<<<<<<< HEAD
using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Services;
=======
using parlayrunner.Shared.Interfaces;
using parlayrunner.Shared.Services;
using parlayrunner.Shared.Models;
using Microsoft.Extensions.Configuration;
>>>>>>> 336064d38e69166b72fa2845b4a15b9cb2a46501

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Configure Supabase settings
var supabaseSettings = new SupabaseSettings();
builder.Configuration.GetSection("Supabase").Bind(supabaseSettings);
builder.Services.AddSingleton(supabaseSettings);

// Add Supabase service
builder.Services.AddScoped<ISupabaseService, SupabaseService>();

// Add User service
builder.Services.AddScoped<IUserService, UserService>();

// Add authentication service
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// Add saegs service
builder.Services.AddScoped<ISaegsService, SaegsService>();

// Add team selection service
builder.Services.AddSingleton<TeamSelectionService>();

// Add user role service
builder.Services.AddSingleton<UserRoleService>();

var app = builder.Build();

// Initialize Supabase
var supabaseService = app.Services.GetRequiredService<ISupabaseService>();
await supabaseService.InitializeAsync();

await app.RunAsync();
