using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Maps;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class TestViewModel : ObservableObject
{

    private readonly IDBService dbService;

    public TestViewModel(IDBService dbService)
    {
        this.dbService = dbService;
    }

    [ObservableProperty]
    private Polyline track = new Polyline
    {
        StrokeColor = Colors.Blue,
        StrokeWidth = 5
    };

    public void LoadData()
    {
        Task.Run(async () =>
        {
            var customTracks = await dbService.GetAllTracksAsync();
            foreach (var customTrack in customTracks)
            {
                customTrack.Locations = await dbService.GetLocationsByTrackIdAsync(customTrack.Id);
                foreach (var location in customTrack.Locations)
                {
                    Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
                }
            }                       
        });
    }
}