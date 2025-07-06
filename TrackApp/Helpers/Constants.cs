﻿namespace TrackApp.Helpers;

public static class Constants
{
    public const string DatabaseFilename = "TrackApp.sqlite";

    public const SQLite.SQLiteOpenFlags Flags =        
        SQLite.SQLiteOpenFlags.ReadWrite |
        SQLite.SQLiteOpenFlags.Create |
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
}