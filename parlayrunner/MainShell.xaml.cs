using AdeptTime.Views;

namespace AdeptTime;

public partial class MainShell : ContentPage
{
    public MainShell()
    {
        InitializeComponent();
        BottomNavigation.SetSelectedTab("kalender");
    }

    private void OnTabSelected(object sender, string tabName)
    {
        ContentView newView = tabName.ToLower() switch
        {
            "kalender" => new KalenderView(),
            "sager" => new SagerView(),
            "ugeseddel" => new UgeseddelView(),
            "indstillinger" => new IndstillingerView(),
            _ => new KalenderView()
        };

        ContentArea.Content = newView;
    }
}
