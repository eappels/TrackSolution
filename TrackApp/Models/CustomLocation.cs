using SQLite;

namespace TrackApp.Models;

public class CustomLocation
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    [Indexed]
    public int CustomTrackId { get; set; }

    public CustomLocation()
    {
    }

    public CustomLocation(double latitude, double longitude, int customTrackId)
    {
        Latitude = latitude;
        Longitude = longitude;
        CustomTrackId = customTrackId;
    }

    public override string ToString()
    {
        return $"Latitude: {Latitude} Longitude: {Longitude} CustomTrackId: {CustomTrackId}";
    }
}