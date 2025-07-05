using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using TrackApp.Messages;
using TrackApp.ViewModels;

namespace TrackApp.Views;

public partial class MapView : ContentPage
{

    private Location location;
    private double zoomLevel = 250;
    private bool isZooming = false;
    private IDispatcherTimer timer;

    public MapView()
	{
		InitializeComponent();

        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(3);
        timer.Tick += (s, e) => RunInBackground();
        timer.Start();

        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            location = await GetCachedLocation();
            if (MyMap != null && location != null)
            {
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMeters(zoomLevel)));
            }
        });

        WeakReferenceMessenger.Default.Register<LocationUpdatedMessage>(this, (r, m) =>
        {
            if (MyMap != null && m.Value != null)
            {
                if (MyMap.MapElements.Count == 0)
                    MyMap.MapElements.Add(((MapViewModel)BindingContext).Track);
                if (!isZooming)
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(m.Value.Latitude, m.Value.Longitude), Distance.FromMeters(zoomLevel)));
            }
        });

        if (MyMap != null)
        {
            MyMap.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "VisibleRegion")
                {
                    isZooming = true;
                    zoomLevel = MyMap.VisibleRegion.Radius.Meters;
                }
            };
        }
    }

    public async Task<Location> GetCachedLocation()
    {
        try
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            if (location != null)
                return location;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving cached location: {ex.Message}");
        }
        return new Location();
    }

    private void RunInBackground()
    {
        isZooming = false;
    }

    public void Dispose()
    {
        timer.Tick -= (s, e) => RunInBackground();
        timer.Stop();
    }
}