using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Threading.Tasks;
using TrackApp.Messages;
using TrackApp.Models;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class MapViewModel : ObservableObject, IDisposable
{

    private readonly IDBService dbService;
    private readonly ILocationService locationService;
    private Location previousLocation;

    public MapViewModel(ILocationService locationService, IDBService dbService)
    {
        previousLocation = new Location(0, 0);
        Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };
        this.locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        this.locationService.OnLocationUpdate += OnLocationUpdate;
        this.dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
    }

    private async void OnLocationUpdate(Location location)
    {
        if (previousLocation.Latitude != 0 && previousLocation.Longitude != 0)
        {
            Distance distance = Distance.BetweenPositions(previousLocation, location);
            if (distance.Meters < 15)
                return;

            if (Track != null)
                Track.Geopath.Add(location);
            await dbService.SaveCustomLocationAsync(new CustomLocation(location.Latitude, location.Longitude, -1));
            WeakReferenceMessenger.Default.Send(new LocationUpdatedMessage(location));
            
            previousLocation = location;
        }
    }

    public void Dispose()
    {
        if (locationService != null)
        {
            locationService.OnLocationUpdate -= OnLocationUpdate;
            locationService.StopTracking();
        }
    }

    [RelayCommand]
    private async void StartStop()
    {
        if (startStopButtonText == "Start")
        {
            locationService.StartTracking();
            StartStopButtonText = "Stop";
            StartStopButtonColor = Colors.Red;
        }
        else
        {
            locationService.StopTracking();
            StartStopButtonText = "Start";
            StartStopButtonColor = Colors.Green;
            if (Track.Geopath.Count > 0)
            {
                var result = await Application.Current.MainPage.DisplayAlert("Save Track", "Do you want to save the current track?", "Yes", "No");
                if (result == true)
                {
                    var locations = Track.Geopath.Select(loc => new CustomLocation
                    {
                        Latitude = loc.Latitude,
                        Longitude = loc.Longitude
                    }).ToList();
                    await dbService.SaveTrackAsync(new CustomTrack(locations));
                    result = await App.Current.Windows[0].Page.DisplayAlert("Track saved", "Do you want to display the saved track?", "Yes", "No");
                    if (result == true)
                    {
                        await Shell.Current.GoToAsync($"///HistoryView", true);
                    }
                }
                Track.Geopath.Clear();
                StartStopButtonColor = Colors.Green;
            }
        }
    }

    [ObservableProperty]
    public Polyline track;

    [ObservableProperty]
    public string startStopButtonText = "Start";

    [ObservableProperty]
    public Color startStopButtonColor = Colors.Green;
}