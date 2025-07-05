using CoreLocation;

namespace TrackApp.Services;

public partial class LocationService
{

    public readonly CLLocationManager locationManager;

    public LocationService()
    {
        locationManager = new CLLocationManager();
        locationManager.PausesLocationUpdatesAutomatically = false;
        locationManager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
        locationManager.AllowsBackgroundLocationUpdates = true;
        locationManager.ActivityType = CLActivityType.AutomotiveNavigation;
    }

    partial void StartTrackingInternal()
    {
        locationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
            OnLocationUpdate?.Invoke(new Location(e.Locations.LastOrDefault().Coordinate.Latitude, e.Locations.LastOrDefault().Coordinate.Longitude));
        locationManager.StartUpdatingLocation();
    }

    partial void StopTrackingInternal()
    {
        locationManager.StopUpdatingLocation();
    }
}