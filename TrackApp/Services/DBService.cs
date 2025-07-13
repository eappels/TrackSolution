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
        return track;
    }

    public async Task<IList<CustomTrack>> GetAllTracksAsync()
    {
        await Init();
        return await database.Table<CustomTrack>().ToListAsync();
    }

    public async Task<CustomTrack> GetTrackbyIdAsync(int id)
    {
        await Init();
        return await database.Table<CustomTrack>()
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IList<CustomLocation>> GetLocationsByTrackIdAsync(int trackId)
    {
        await Init();
        return await database.Table<CustomLocation>()
            .Where(l => l.CustomTrackId == trackId)
            .ToListAsync();
    }

    public async Task<int> DeleteTrackAsync(CustomTrack track)
    {
        await Init();
        var locations = await database.Table<CustomLocation>()
            .Where(l => l.CustomTrackId == track.Id)
            .ToListAsync();
        foreach (var location in locations)
        {
            await database.DeleteAsync(location);
        }
        return await database.DeleteAsync(track);
    }

    public async Task ClearDatabase()
    {
        await Init();
        await database.DropTableAsync<CustomTrack>();
        await database.DropTableAsync<CustomLocation>();
        await database.CreateTableAsync<CustomTrack>();
        await database.CreateTableAsync<CustomLocation>();
    }
}