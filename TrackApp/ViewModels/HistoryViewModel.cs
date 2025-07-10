using CommunityToolkit.Mvvm.ComponentModel;
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

    public HistoryViewModel()
    {
        this.dbService = new Services.DBService();
        Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            Tracks = await dbService.GetAllTracksAsync();
            foreach (var item in track)
            {
                Debug.WriteLine(item.ToString());
            }
        });
    }

    //public HistoryViewModel(IDBService dbService)
    //{
    //    this.dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
    //    Track = new Polyline
    //    {
    //        StrokeColor = Colors.Blue,
    //        StrokeWidth = 5
    //    };

    //    MainThread.BeginInvokeOnMainThread(async () =>
    //    {
    //        Tracks = await dbService.GetAllTracksAsync();
    //        foreach (var item in track)
    //        {
    //            Debug.WriteLine(item.ToString());
    //        }
    //    });
    //}

    async partial void OnSelectedTrackChanged(CustomTrack value)
    {
        Track.Geopath.Clear();

        if (value == null)
        {
            Debug.WriteLine($"value: {value}");
            return;
        }

        if (value.Locations == null)
        {
            Debug.WriteLine($"value.Locations == null: {value.Locations == null}");
            value.Locations = await dbService.GetLocationsByTrackIdAsync(value.Id);
        }
        foreach (var location in value.Locations)
        {
            Debug.WriteLine($"Locations: {location.ToString()}");
            Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
        }
        WeakReferenceMessenger.Default.Send(new HistoryTrackSelectedChangedMessage(value));
    }


    [ObservableProperty]
    private Polyline track;

    [ObservableProperty]
    private CustomTrack selectedTrack;

    [ObservableProperty]
    private List<CustomTrack> tracks;
}