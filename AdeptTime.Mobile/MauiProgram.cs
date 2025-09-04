using Microsoft.Extensions.Logging;
using AdeptTime;
using AdeptTime.Shared.Services;
using AdeptTime.Shared.Interfaces;
using AdeptTime.Shared.Models;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Configure Supabase settings for mobile
        var supabaseSettings = new SupabaseSettings
        {
            Url = "http://127.0.0.1:54321", // Local development Supabase
            Key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6ImFub24iLCJleHAiOjE5ODM4MTI5OTZ9.CRXP1A7WOeoJeXxjNni43kdQwgnWNReilDMblYTn_I0"
        };
        builder.Services.AddSingleton(supabaseSettings);

        // Add shared services
        builder.Services.AddScoped<ISupabaseService, SupabaseService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<ITeamService, TeamService>();
        builder.Services.AddScoped<ITimeRegistrationService, TimeRegistrationService>();
        builder.Services.AddSingleton<UserRoleService>();
        
        // Add mobile ViewModels
        builder.Services.AddTransient<AdeptTime.Mobile.ViewModels.LoginViewModel>();
        
        // Add mobile Views
        builder.Services.AddTransient<AdeptTime.Views.LoginView>();
        builder.Services.AddTransient<AdeptTime.Views.KalenderView>();

        return builder.Build();
    }
}