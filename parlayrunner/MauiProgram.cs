using Microsoft.Extensions.Logging;
using parlayrunner;
using parlayrunner.Shared.Interfaces;
using parlayrunner.Shared.Services;
using parlayrunner.Shared.ViewModels;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        // Services
        builder.Services.AddSingleton<ICloudService, CloudService>();
        builder.Services.AddSingleton<IAccountService, AccountService>();
        builder.Services.AddSingleton<IParlayService, ParlayService>();

        // Register HomeViewModel as transient
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<ParlayViewModel>();

        return builder.Build();
    }
}