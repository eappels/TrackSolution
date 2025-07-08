using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class DevViewModel : ObservableObject
{

    private readonly IDBService dbService;

    public DevViewModel(IDBService dbService)
    {
        this.dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
        Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };
    }

    [RelayCommand]
    private async Task Dev()
    {
        var lastTrack = await dbService.ReadLastTrackAsync();
        Track.Geopath.Clear();
        foreach (var location in lastTrack.Locations)
        {
            Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
        }
    }

    [ObservableProperty]
    private Polyline track;
}