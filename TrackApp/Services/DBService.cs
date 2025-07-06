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
        if (File.Exists(Constants.DatabasePath))
            File.Delete(Constants.DatabasePath);
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
        if (track.Id > 0 && track.Locations != null)
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
        // return the last track
        var tracks = await database.Table<CustomTrack>().OrderByDescending(t => t.Id).ToListAsync();
        if (tracks.Count == 0)
            return null;
        return tracks[0];
    }
}