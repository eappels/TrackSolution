using SQLite;
using TrackApp.Services.Interfaces;

namespace TrackApp.Models;

public class CustomTrack
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Ignore]
    public List<CustomLocation> Locations { get; set; }
    private readonly IDBService dbService;

    public CustomTrack()
    {
    }

    public CustomTrack(IDBService dbService)
    {
        this.dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
    }

    public CustomTrack(IList<Location> locations)
    {
        Locations = new List<CustomLocation>();
        foreach (var location in locations)
        {
            Locations.Add(new CustomLocation
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            });
        }
    }
}