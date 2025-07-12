using SQLite;

namespace TrackApp.Models;

public class CustomTrack
{

    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }

    [Ignore]
    public List<CustomLocation> Locations { get; set; } = new();

    public CustomTrack()
    {
    }

    public CustomTrack(List<CustomLocation> locations)
    {
        Locations = locations;
    }
}