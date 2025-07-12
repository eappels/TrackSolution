using TrackApp.Models;

namespace TrackApp.Services.Interfaces;

public interface IDBService
{
    Task<int> SaveTrackAsync(CustomTrack track);
    Task<CustomTrack> ReadLastTrackAsync();
    Task<CustomTrack> GetTrackbyIdAsync(int id);
    Task<List<CustomTrack>> GetAllTracksAsync();
    Task<List<CustomLocation>> GetLocationsByTrackIdAsync(int trackId);
    Task ClearDatabase();
}