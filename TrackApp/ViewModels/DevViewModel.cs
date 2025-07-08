using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class DevViewModel : ObservableObject
{

    private readonly IDBService dbService;

    public DevViewModel(IDBService dbService)
    {
        this.dbService = dbService ?? throw new ArgumentNullException(nameof(dbService));
    }

    [RelayCommand]
    private void Dev()
    {
        var tracks = dbService.GetAllTracksAsync();
        if (tracks.Result.Count > 0)
        {
            var lastTrack = tracks.Result[^1];
            Debug.WriteLine($"Last track ID: {lastTrack.Id}");
            Debug.WriteLine($"Last track locations count: {lastTrack.Locations.Count}");
        }
        else
        {
            Debug.WriteLine("No tracks found in the database.");
        }
    }
}