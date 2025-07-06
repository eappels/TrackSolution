using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using System.Diagnostics;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class HistoryViewModel : ObservableObject, IDisposable
{

    private readonly IDBService dbService;

    public HistoryViewModel(IDBService dbService)
    {
        this.dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
        Track = new Polyline()
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };
    }

    public async void LoadHistoryView()
    {
        var mytrack = await dbService.ReadLastTrackAsync();
        if (mytrack == null || mytrack.Locations == null || mytrack.Locations.Count == 0)
        {
            Debug.WriteLine("error");
            return;
        }
        foreach (var location in mytrack.Locations)
        {
            Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
        }
    }

    public void Dispose()
    {
        
    }

    [ObservableProperty]
    private Polyline track;
}