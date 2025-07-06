using SQLite;
using System.Diagnostics;
using TrackApp.Helpers;
using TrackApp.Models;
using TrackApp.Services.Interfaces;

namespace TrackApp.Services;

public class DBService : IDBService
{

    private SQLiteAsyncConnection database;

    public DBService()
    {
        //if (File.Exists(Constants.DatabasePath))
        //    File.Delete(Constants.DatabasePath);
    }

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
        int result = await database.InsertAsync(track);
        if (track.Locations != null && track.Locations.Count > 0)
        {
            foreach (var location in track.Locations)
            {
                location.CustomTrackId = track.Id;
                await database.InsertAsync(location);
            }
        }
        return result;
    }

    public async Task<CustomTrack> ReadLastTrackAsync()
    {
        await Init();
        var tracks = await database.Table<CustomTrack>().OrderByDescending(t => t.Id).ToListAsync();
        if (tracks.Count == 0)
            return null;
        Debug.WriteLine($"Last track ID: {tracks[0].Id}");
        Debug.WriteLine($"Last track locations count: {tracks[0].Locations.Count}");
        return tracks[0];
    }
}