using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using TrackApp.Messages;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class MapViewModel : ObservableObject, IDisposable
{

    private readonly ILocationService locationService;

    public MapViewModel(ILocationService locationService)
    {
        Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };
        this.locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        this.locationService.OnLocationUpdate += OnLocationUpdate;
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
    private Polyline track;

    [ObservableProperty]
    private string startStopButtonText = "Start";

    [ObservableProperty]
    private Color startStopButtonColor = Colors.Green;

    [RelayCommand]
    private void StartStop()
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
        }
    }
}