using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using AdeptTime.Web;
using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Services;
using AdeptTime.Shared.Models;

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

// Add team services
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITimeRegistrationService, TimeRegistrationService>();

// Add team selection service
builder.Services.AddSingleton<TeamSelectionService>();

// Add user role service
builder.Services.AddSingleton<UserRoleService>();

var app = builder.Build();

// Initialize Supabase
var supabaseService = app.Services.GetRequiredService<ISupabaseService>();
await supabaseService.InitializeAsync();

// Seed default admin user in debug mode (silent operation)
#if DEBUG
try
{
    var userService = app.Services.GetRequiredService<IUserService>();
    await userService.SeedDefaultAdminUserAsync();
}
catch (Exception)
{
    // Silently ignore seeding errors - this is expected when Supabase is not running
    // The application will work in demo mode
}
#endif

await app.RunAsync();
