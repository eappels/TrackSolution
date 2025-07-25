﻿using TrackApp.Models;

namespace TrackApp.Services.Interfaces;

public interface IDBService
{
    Task<int> SaveTrackAsync(CustomTrack track);
    Task<CustomTrack> ReadLastTrackAsync();
    Task<CustomTrack> GetTrackbyIdAsync(int id);
    Task<IList<CustomTrack>> GetAllTracksAsync();
    Task<IList<CustomLocation>> GetLocationsByTrackIdAsync(int trackId);
    Task<int> DeleteTrackAsync(CustomTrack track);
    Task ClearDatabase();
}