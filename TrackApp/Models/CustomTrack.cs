using SQLite;

namespace TrackApp.Models;

public class CustomTrack
{

    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }

    [Ignore]
    public List<CustomLocation> Locations { get; set; }

    public CustomTrack()
    {
    }

    public CustomTrack(List<CustomLocation> Locations)
    {
        this.Locations = Locations ?? new List<CustomLocation>();
    }
}