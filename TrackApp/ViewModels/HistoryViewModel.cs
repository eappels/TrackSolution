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
    private int currentTrackIndex = -1;

    public HistoryViewModel(IDBService dbServcice)
    {
        this.dbService = dbServcice;
    }

    public async Task LoadDataFromDatabase()
    {
        Track.Geopath.Clear();
        Tracks = await dbService.GetAllTracksAsync();

        if (Tracks.Count > 0)
        {
            SelectedTrack = Tracks.LastOrDefault();
            if (SelectedTrack is not null)
            {
                currentTrackIndex = SelectedTrack.Id;
                SelectedTrack.Locations = await dbService.GetLocationsByTrackIdAsync(currentTrackIndex);
                foreach (var location in SelectedTrack.Locations)
                    Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
            }
        }
        else
        {
            SelectedTrack = null;
            currentTrackIndex = -1;
        }
    }

    async partial void OnSelectedTrackChanged(CustomTrack value)
    {        
        if (value is null)
            return;

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
    private void LoadNext()
    {
        if (Tracks.Count == 0)
            return;
        var firstTrack = Tracks.FirstOrDefault();
        if (firstTrack is null)
            return;
        SelectedTrack = firstTrack;
    }

    [RelayCommand]
    private void LoadPrevious()
    {
        if (Tracks.Count == 0)
            return;
        var lastTrack = Tracks.LastOrDefault();
        if (lastTrack is null)
            return;
        SelectedTrack = lastTrack;
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