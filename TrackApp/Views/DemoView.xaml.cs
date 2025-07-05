using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using TrackApp.Messages;
using TrackApp.ViewModels;

namespace TrackApp.Views
{
    public partial class DemoView : ContentPage
    {

        private Location location;

        public DemoView()
        {
            InitializeComponent();

            MainThread.InvokeOnMainThreadAsync(async () =>
            {
                location = await GetCachedLocation();
                if (MyMap != null && location != null)
                {
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMeters(250)));
                }
            });

            WeakReferenceMessenger.Default.Register<LocationUpdatedMessage>(this, (r, m) =>
            {
                if (MyMap != null && m.Value != null)
                {
                    if (MyMap.MapElements.Count == 0)
                        MyMap.MapElements.Add(((DemoViewModel)BindingContext).Track);
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(m.Value.Latitude, m.Value.Longitude), Distance.FromMeters(250)));
                }
            });
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
    }
}