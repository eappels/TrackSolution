using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using System.Diagnostics;
using TrackApp.Models;
using TrackApp.Services.Interfaces;

namespace TrackApp.ViewModels;

public partial class TestViewModel : ObservableObject
{

    private readonly IDBService dbService;
    private CustomTrack latestTrack;
    private CustomTrack currentTrack;

    public TestViewModel(IDBService dbService)
    {
        this.dbService = dbService;
    }

    public async void LoadData()
    {
        await LoadTrack();
    }

    [RelayCommand]
    private async Task PreviousTrack()
    {
        await LoadTrack(latestTrack.Id - 1);
    }

    [RelayCommand]
    private async Task NextTrack()
    {
        await LoadTrack(latestTrack.Id + 1);
    }

    private async Task LoadTrack()
    {
        try
        {
            latestTrack = await dbService.ReadLastTrackAsync();
            Debug.WriteLine($"Loaded track with ID: {latestTrack.Id}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading track with ID {latestTrack.Id}: {ex.Message}");
            throw;
        }        
        Track.Geopath.Clear();
        if (latestTrack == null)
        {
            Debug.WriteLine("No track found.");
            return;
        }
        if (latestTrack.Locations.Count > 0)
        {
            foreach (var location in latestTrack.Locations)
            {
                Track.Geopath.Add(new Location(location.Latitude, location.Longitude));
            }
        }
    }

    private async Task LoadTrack(int id)
    {
        await Task.Delay(1000); // Simulate loading delay
    }

    [ObservableProperty]
    private Polyline track = new Polyline
    {
        StrokeColor = Colors.Blue,
        StrokeWidth = 5
    };
}