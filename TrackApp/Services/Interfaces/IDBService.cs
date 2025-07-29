using TrackApp.Models;

namespace TrackApp.Services.Interfaces;

public interface IDBService
{
    Task<int> SaveTrackAsync(CustomTrack track);
    Task<CustomTrack> GetLastTrackAsync();
    Task<CustomTrack> GetTrackbyIdAsync(int id);
    Task<IList<CustomTrack>> GetAllTracksAsync();
    Task<IList<CustomTrack>> GetTracksAsync(int limit, int offset);
    Task<IList<CustomLocation>> GetLocationsByTrackIdAsync(int trackId);
    Task<int> DeleteTrackAsync(CustomTrack track);
    Task ClearDatabase();
}