using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using System.Diagnostics;
using TrackApp.Messages;
using TrackApp.Models;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class MapViewModel : ObservableObject, IDisposable
{

    private readonly IDBService dbService;
    private readonly ILocationService locationService;

    public MapViewModel(ILocationService locationService, IDBService dbService)
    {
        Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };
        this.locationService = locationService;
        this.locationService.OnLocationUpdate += OnLocationUpdate;
        this.dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));


        Task.Run(async () =>
        {
            var tracksInDB = await dbService.GetAllTracksAsync();
            foreach (CustomTrack track in tracksInDB)
            {
                Debug.WriteLine($"Track ID: {track.Id}");
                var cLocations = await dbService.GetLocationsByTrackIdAsync(track.Id);
                foreach (CustomLocation cLocation in cLocations)
                {
                    Debug.WriteLine($"Location: {cLocation.Latitude}, {cLocation.Longitude}");
                }
            }
        });
    }

    private void OnLocationUpdate(Location location)
    {
        if (Track != null)
            Track.Geopath.Add(location);
        WeakReferenceMessenger.Default.Send(new LocationUpdatedMessage(location));
    }

    public void Dispose()
    {
        if (locationService != null)
        {
            locationService.OnLocationUpdate -= OnLocationUpdate;
            locationService.StopTracking();
        }
    }

    [ObservableProperty]
    public Polyline track;

    [ObservableProperty]
    public string startStopButtonText = "Start";

    [ObservableProperty]
    public Color startStopButtonColor = Colors.Green;

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
}