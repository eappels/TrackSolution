using TrackApp.Models;

namespace TrackApp.Services.Interfaces;

public interface IDBService
{
    Task<int> SaveTrackAsync(CustomTrack track);
    Task<CustomTrack> ReadLastTrackAsync();
    Task<List<CustomTrack>> GetAllTracksAsync();
}