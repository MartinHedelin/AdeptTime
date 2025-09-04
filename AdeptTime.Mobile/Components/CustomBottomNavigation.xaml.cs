namespace AdeptTime.Components;

public partial class CustomBottomNavigation : ContentView
{
    public event EventHandler<string>? TabSelected;

    public CustomBottomNavigation()
    {
        InitializeComponent();
    }

    public void SetSelectedTab(string tabName)
    {
        // Hide all borders and show all default states first
        KalenderBorder.IsVisible = false;
        KalenderDefault.IsVisible = true;
        SagerBorder.IsVisible = false;
        SagerDefault.IsVisible = true;
        UgeseddelBorder.IsVisible = false;
        UgeseddelDefault.IsVisible = true;
        IndstillingerBorder.IsVisible = false;
        IndstillingerDefault.IsVisible = true;

        // Show the selected tab's border and hide its default state
        switch (tabName.ToLower())
        {
            case "kalender":
                KalenderBorder.IsVisible = true;
                KalenderDefault.IsVisible = false;
                break;
            case "sager":
                SagerBorder.IsVisible = true;
                SagerDefault.IsVisible = false;
                break;
            case "ugeseddel":
                UgeseddelBorder.IsVisible = true;
                UgeseddelDefault.IsVisible = false;
                break;
            case "indstillinger":
                IndstillingerBorder.IsVisible = true;
                IndstillingerDefault.IsVisible = false;
                break;
        }
    }

    private void OnKalenderTapped(object sender, EventArgs e)
    {
        SetSelectedTab("kalender");
        TabSelected?.Invoke(this, "kalender");
    }

    private void OnSagerTapped(object sender, EventArgs e)
    {
        SetSelectedTab("sager");
        TabSelected?.Invoke(this, "sager");
    }

    private void OnUgeseddelTapped(object sender, EventArgs e)
    {
        SetSelectedTab("ugeseddel");
        TabSelected?.Invoke(this, "ugeseddel");
    }

    private void OnIndstillingerTapped(object sender, EventArgs e)
    {
        SetSelectedTab("indstillinger");
        TabSelected?.Invoke(this, "indstillinger");
    }
}
