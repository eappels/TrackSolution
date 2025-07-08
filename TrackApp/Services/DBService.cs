using SQLite;
using System.Diagnostics;
using TrackApp.Helpers;
using TrackApp.Models;
using TrackApp.Services.Interfaces;

namespace TrackApp.Services;

public class DBService : IDBService
{

    private SQLiteAsyncConnection database;

    async Task Init()
    {
        if (database is not null)
            return;
        database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        Debug.WriteLine($"Database path: {Constants.DatabasePath}");
        await database.CreateTableAsync<CustomTrack>();
        await database.CreateTableAsync<CustomLocation>();
    }

    public async Task<int> SaveTrackAsync(CustomTrack track)
    {
        await Init();
        var i = await database.InsertAsync(track);
        foreach (CustomLocation location in track.Locations)
        {
            location.CustomTrackId = track.Id;
            await database.InsertAsync(location);
        }
        return i;
    }

    public async Task<CustomTrack> ReadLastTrackAsync()
    {
        await Init();
        var track = await database.Table<CustomTrack>()
            .OrderByDescending(t => t.Id)
            .FirstOrDefaultAsync();
        track.Locations = await database.Table<CustomLocation>()
            .Where(l => l.CustomTrackId == track.Id)
            .ToListAsync();

        Debug.WriteLine($"Last track ID: {track.Id}");
        Debug.WriteLine($"Last track locations count: {track.Locations.Count}");
        return track;
    }

    public async Task<List<CustomTrack>> GetAllTracksAsync()
    {
        await Init();
        var tracks = new List<CustomTrack>();
        try
        {
            tracks = await database.Table<CustomTrack>().ToListAsync();
            Debug.WriteLine($"Tracks count: {tracks.Count}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving tracks: {ex.Message}");
            throw;
        }        
        if (tracks == null || tracks.Count == 0)
        {
            Debug.WriteLine("No tracks found in the database.");
            return new List<CustomTrack>();
        }
        Debug.WriteLine($"Total tracks found: {tracks.Count}");
        return tracks;
    }
}