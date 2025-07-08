using Microsoft.Extensions.Logging;
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
        builder.Services.AddTransient<MapView>(s => new MapView
        {
            BindingContext = s.GetRequiredService<MapViewModel>()
        });

        builder.Services.AddSingleton<DevViewModel>();
        builder.Services.AddTransient<DevView>(s => new DevView
        {
            BindingContext = s.GetRequiredService<DevViewModel>()
        });

        return builder.Build();
    }
}