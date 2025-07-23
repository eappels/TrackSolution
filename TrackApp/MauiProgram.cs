using Microsoft.Extensions.Logging;
using TrackApp.Helpers;
using TrackApp.Services;
using TrackApp.Services.Interfaces;
using TrackApp.ViewModels;
using TrackApp.Views;

namespace TrackApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton<IDBService, DBService>();
        builder.Services.AddSingleton<ILocationService, LocationService>();

        builder.Services.AddSingleton<MapViewModel>();
        builder.Services.AddTransient<MapView>();

        builder.Services.AddSingleton<HistoryViewModel>();
        builder.Services.AddTransient<HistoryView>();

        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddTransient<SettingsView>();

        var app = builder.Build();
        ServiceHelper.Initialize(app.Services);

        return app;
    }
}