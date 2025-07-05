using SQLite;

namespace TrackApp.Models;

public class CustomLocation
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public CustomLocation()
    {        
    }

    public CustomLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}