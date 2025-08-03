using SQLite;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
        var savedLocations = await database.Table<CustomLocation>()
            .Where(l => l.CustomTrackId == -1)
            .ToListAsync();
        foreach (CustomLocation location in savedLocations)
        {
            location.CustomTrackId = track.Id;
            await database.InsertAsync(location);
        }

        //await database.ExecuteAsync("DELETE FROM CustomLocation WHERE CustomTrackId = -1");
        foreach (var oldLocation in savedLocations)
        {
            await database.DeleteAsync(oldLocation);
        }
        return i;
    }

    public async Task<int> SaveCustomLocationAsync(CustomLocation location)
    {
        await Init();
        if (location != null)
        {
            return await database.InsertAsync(location);
        }
        return 0;
    }

    public async Task<CustomTrack> GetLastTrackAsync()
    {
        await Init();
        var track = await database.Table<CustomTrack>()
            .OrderByDescending(t => t.Id)
            .FirstOrDefaultAsync();
        await LoadLocations(track);
        return track;
    }

    private async Task LoadLocations(CustomTrack track)
    {
        await Init();
        track.Locations = await database.Table<CustomLocation>()
            .Where(l => l.CustomTrackId == track.Id)
            .ToListAsync();
    }

    public async Task<IList<CustomTrack>> GetAllTracksAsync()
    {
        await Init();
        return await database.Table<CustomTrack>().ToListAsync();
    }

    public async Task<IList<CustomTrack>> GetTracksAsync(int limit, int offset)
    {
        await Init();
        return await database.Table<CustomTrack>()
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<CustomTrack> GetTrackbyIdAsync(int id)
    {
        await Init();
        var track = await database.Table<CustomTrack>()
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync();
        await LoadLocations(track);
        return track;
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