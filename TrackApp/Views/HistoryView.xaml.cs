using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;
using TrackApp.Helpers;
using TrackApp.Messages;
using TrackApp.ViewModels;

namespace TrackApp.Views;

public partial class HistoryView : ContentPage
{

    private HistoryViewModel viewModel;

    public HistoryView()
	{
		InitializeComponent();

        BindingContext = viewModel = ServiceHelper.GetService<HistoryViewModel>();

        MyMap.MapElements.Add(viewModel.Track);

        WeakReferenceMessenger.Default.Register<HistoryTrackSelectedChangedMessage>(this, (r, m) =>
		{
            if (MyMap != null && m.Value?.Locations?.Count > 0)
			{
				var locations = m.Value.Locations;
				double minLat = locations.Min(l => l.Latitude);
				double maxLat = locations.Max(l => l.Latitude);
				double minLon = locations.Min(l => l.Longitude);
				double maxLon = locations.Max(l => l.Longitude);

				const double paddingFactor = 1.2;

				double latSpan = Math.Max(0.01, maxLat - minLat) * paddingFactor;
				double lonSpan = Math.Max(0.01, maxLon - minLon) * paddingFactor;

				var bounds = new MapSpan(
					new Location(
						(minLat + maxLat) / 2,
						(minLon + maxLon) / 2),
					latSpan,
					lonSpan
				);

				MyMap.MoveToRegion(bounds);
			}
		});        
    }
}