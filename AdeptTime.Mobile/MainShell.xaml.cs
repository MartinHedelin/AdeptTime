using AdeptTime.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AdeptTime;

public partial class MainShell : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public MainShell(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        BottomNavigation.SetSelectedTab("kalender");
        
        // Set initial content
        var kalenderView = _serviceProvider.GetRequiredService<KalenderView>();
        ContentArea.Content = kalenderView.Content;
    }

    private void OnTabSelected(object sender, string tabName)
    {
        ContentPage newView = tabName.ToLower() switch
        {
            "kalender" => _serviceProvider.GetRequiredService<KalenderView>(),
            "sager" => _serviceProvider.GetRequiredService<SagerView>(),
            "ugeseddel" => _serviceProvider.GetRequiredService<UgeseddelView>(),
            "indstillinger" => _serviceProvider.GetRequiredService<IndstillingerView>(),
            _ => _serviceProvider.GetRequiredService<KalenderView>()
        };

        ContentArea.Content = newView.Content;
    }
}
