using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    }

    public async Task LoadDataFromDatabase()
    {
        Track.Geopath.Clear();
        Tracks = await dbService.GetAllTracksAsync();
    }

    async partial void OnSelectedTrackChanged(CustomTrack value)
    {        
        if (value is null)
            return;

        if (Track.Geopath.Count > 0)
            Track.Geopath.Clear();

        value.Locations = await dbService.GetLocationsByTrackIdAsync(value.Id);
        foreach (var location in value.Locations)
            Track.Geopath.Add(new Location(location.Latitude, location.Longitude));

        WeakReferenceMessenger.Default.Send(new HistoryTrackSelectedChangedMessage(value));
    }

    [RelayCommand]
    private async void Delete()
    {
        if (SelectedTrack is null)
            return;

        var data = await dbService.DeleteTrackAsync(SelectedTrack);

        SelectedTrack = null;
        Track.Geopath.Clear();
        Tracks.Clear();
        Tracks = await dbService.GetAllTracksAsync();
    }

    [RelayCommand]
    private void FullScreen()
    {

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
    private IList<CustomTrack> tracks;
}