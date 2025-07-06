using Microsoft.Maui.Maps;
using System.Threading.Tasks;
using TrackApp.ViewModels;

namespace TrackApp.Views;

public partial class HistoryView : ContentPage
{


    private double zoomLevel = 150;

    public HistoryView()
	{
		InitializeComponent();
    }

    protected new async Task OnAppearing()
    {
        base.OnAppearing();
        ((HistoryViewModel)BindingContext).LoadHistoryView();
        if (MyMap != null)
        {
            MyMap.MapElements.Add(((HistoryViewModel)BindingContext).Track);

            await Task.Delay(1000);

            var geopath = ((HistoryViewModel)BindingContext).Track.Geopath;
            if (geopath != null && geopath.Count > 0)
            {
                double minLat = geopath[0].Latitude, maxLat = geopath[0].Latitude;
                double minLon = geopath[0].Longitude, maxLon = geopath[0].Longitude;

                foreach (var position in geopath)
                {
                    if (position.Latitude < minLat) minLat = position.Latitude;
                    if (position.Latitude > maxLat) maxLat = position.Latitude;
                    if (position.Longitude < minLon) minLon = position.Longitude;
                    if (position.Longitude > maxLon) maxLon = position.Longitude;
                }

                var southwest = new Location(minLat, minLon);
                var northeast = new Location(maxLat, maxLon);
                var bounds = new MapSpan(
                    new Location((minLat + maxLat) / 2, (minLon + maxLon) / 2),
                    Math.Abs(maxLat - minLat) * 1.2, // Add some padding
                    Math.Abs(maxLon - minLon) * 1.2
                );

                MyMap.MoveToRegion(bounds);
            }
        }
    }
}