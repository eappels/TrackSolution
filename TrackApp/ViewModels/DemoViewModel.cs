using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using TrackApp.Messages;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class DemoViewModel : ObservableObject, IDisposable
{

    private readonly ILocationService locationService;

    public DemoViewModel(ILocationService locationService)
    {
        this.locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        this.locationService.OnLocationUpdate += OnLocationUpdate;
        this.locationService.StartTracking();
    }      

    private void OnLocationUpdate(Location location)
    {
        Debug.WriteLine($"Location updated: {location.Latitude}, {location.Longitude}");
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
}