using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using System.Diagnostics;
using TrackApp.Models;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class DevViewModel : ObservableObject
{

    private readonly IDBService dbService;
    private int currentTrack = 0;

    public DevViewModel(IDBService dbService)
    {
        this.dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
        Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            Tracks = await dbService.GetAllTracksAsync();
        });
    }

    [RelayCommand]
    private async void ShowTrack()
    {

        Debug.WriteLine($"currentTrack: {currentTrack}");
        var track = new CustomTrack();
        if (currentTrack != 0)
        {
            track = Tracks[currentTrack];            
        }
        else
        {
            track = await dbService.ReadLastTrackAsync();
        }
        track.Locations = await dbService.GetLocationsByTrackIdAsync(track.Id);
        currentTrack = track.Id;
        Track.Geopath.Clear();
        foreach (var location in track.Locations)
        {
            Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
        }
    }

    async partial void OnSelectedTrackChanged(CustomTrack value)
    {
        Track.Geopath.Clear();
        
        if (value == null)
            return;

        if (value.Locations == null)
        {
            value.Locations = await dbService.GetLocationsByTrackIdAsync(value.Id);
        }
        foreach (var location in value.Locations)
        {
            Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
        }
    }


    [ObservableProperty]
    private Polyline track;

    [ObservableProperty]
    private CustomTrack selectedTrack;

    [ObservableProperty]
    private List<CustomTrack> tracks;
}