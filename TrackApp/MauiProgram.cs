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
        builder.Services.AddSingleton<ILocationService, LocationService>();

        builder.Services.AddSingleton<DemoViewModel>();
        builder.Services.AddTransient<DemoView>(s => new DemoView
        {
            BindingContext = s.GetRequiredService<DemoViewModel>()
        });

        return builder.Build();
    }
}