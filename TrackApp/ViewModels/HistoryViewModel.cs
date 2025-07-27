using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using System.Diagnostics;
using TrackApp.Messages;
using TrackApp.Models;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class HistoryViewModel : ObservableObject
{

    private readonly IDBService dbService;
    private int limit = 1, offset = 0;

    public HistoryViewModel(IDBService dbServcice)
    {
        this.dbService = dbServcice;
    }

    public async Task LoadDataFromDatabase()
    {
        Track.Geopath.Clear();
        Tracks = await dbService.GetTracksAsync(limit, offset);

        if (Tracks.Count == 1)
        {
            var track = Tracks[0];
            if (track.Locations is null || track.Locations.Count == 0)
                track.Locations = await dbService.GetLocationsByTrackIdAsync(track.Id);
            foreach (var location in track.Locations)
                Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
            WeakReferenceMessenger.Default.Send(new HistoryTrackSelectedChangedMessage(track));
        }
        else
        {
            Debug.WriteLine($"Tracks count: {Tracks.Count}");
            throw new Exception("Tracks count is not 1, something went wrong.");
        }
    }

    [RelayCommand]
    private async Task Next()
    {
        offset += limit;
        var data = await dbService.GetTracksAsync(limit, offset);
        if (data != null && data.Count > 0)
        {
            Track.Geopath.Clear();
            foreach (var track in data)
            {
                if (track.Locations is null || track.Locations.Count == 0)
                    track.Locations = await dbService.GetLocationsByTrackIdAsync(track.Id);
                foreach (var location in track.Locations)
                    Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
                SelectedTrack = track;
                WeakReferenceMessenger.Default.Send(new HistoryTrackSelectedChangedMessage(track));
            }
        }
        else
        {
            offset -= limit;
        }
    }

    [RelayCommand]
    private async Task Previous()
    {
        offset -= limit;
        var data = await dbService.GetTracksAsync(limit, offset);

        if (data != null && data.Count > 0)
        {
            Track.Geopath.Clear();
            foreach (var track in data)
            {
                if (track.Locations is null || track.Locations.Count == 0)
                    track.Locations = await dbService.GetLocationsByTrackIdAsync(track.Id);
                foreach (var location in track.Locations)
                    Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
                SelectedTrack = track;
                WeakReferenceMessenger.Default.Send(new HistoryTrackSelectedChangedMessage(track));
            }
        }
        else
        {
            offset += limit;
        }
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
        await LoadDataFromDatabase();
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

    [ObservableProperty]
    private int currentIndex;
}