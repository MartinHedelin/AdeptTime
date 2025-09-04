using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace AdeptTime.Views;

public partial class KalenderView : ContentPage
{
    public KalenderView()
    {
        InitializeComponent();
        InitializeMap();
    }

    private void InitializeMap()
    {
        // Set map to Copenhagen area to match reference design
        var copenhagenLocation = new Location(55.6761, 12.5683);
        var mapSpan = MapSpan.FromCenterAndRadius(copenhagenLocation, Distance.FromKilometers(10));
        map.MoveToRegion(mapSpan);
    }
}
