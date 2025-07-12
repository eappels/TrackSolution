using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using TrackApp.Messages;
using TrackApp.Models;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class HistoryViewModel : ObservableObject
{

    private readonly IDBService dbService;

    public HistoryViewModel()
    {
        this.dbService = new Services.DBService();
        Task.Run(async () =>
        {
            Tracks = await dbService.GetAllTracksAsync();
        });
    }

    async partial void OnSelectedTrackChanged(CustomTrack value)
    {
        Track.Geopath.Clear();

        value.Locations = await dbService.GetLocationsByTrackIdAsync(value.Id);
        foreach (var location in value.Locations)
        {
            Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
        }
        WeakReferenceMessenger.Default.Send(new HistoryTrackSelectedChangedMessage(value));
    }


    [ObservableProperty]
    private Polyline track = new Polyline
    {
        StrokeColor = Colors.Blue,
        StrokeWidth = 5
    };

    [ObservableProperty]
    private CustomTrack selectedTrack;

    [ObservableProperty]
    private List<CustomTrack> tracks;
}