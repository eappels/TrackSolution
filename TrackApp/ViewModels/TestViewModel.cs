using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Maps;
using TrackApp.Models;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class TestViewModel : ObservableObject
{

    private readonly IDBService dbService;
    private CustomTrack latestTrack;

    public TestViewModel(IDBService dbService)
    {
        this.dbService = dbService;
    }

    public void LoadData()
    {
        Task.Run(async () =>
        {
            Tracks = await dbService.GetAllTracksAsync();
            latestTrack = Tracks[int.MaxValue];
            latestTrack.Locations = await dbService.GetLocationsByTrackIdAsync(latestTrack.Id);
            foreach (var location in latestTrack.Locations)
            {
                Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
            }
        });
    }

    [ObservableProperty]
    private Polyline track = new Polyline
    {
        StrokeColor = Colors.Blue,
        StrokeWidth = 5
    };

    [ObservableProperty]
    private IList<CustomTrack> tracks;
}