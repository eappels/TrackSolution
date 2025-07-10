using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using TrackApp.Messages;
using TrackApp.ViewModels;

namespace TrackApp.Views;

public partial class HistoryView : ContentPage
{
	public HistoryView()
	{
		InitializeComponent();


		if (BindingContext is not HistoryViewModel)
			BindingContext = new HistoryViewModel(new Services.DBService());

		MyMap.MapElements.Add(((HistoryViewModel)BindingContext).Track);

        WeakReferenceMessenger.Default.Register<HistoryTrackSelectedChangedMessage>(this, (r, m) =>
		{
			if (MyMap != null && m.Value?.Locations?.Count > 0)
			{
				var locations = m.Value.Locations;
				double minLat = locations.Min(l => l.Latitude);
				double maxLat = locations.Max(l => l.Latitude);
				double minLon = locations.Min(l => l.Longitude);
				double maxLon = locations.Max(l => l.Longitude);

				var southwest = new Location(minLat, minLon);
				var northeast = new Location(maxLat, maxLon);
				var bounds = new MapSpan(
					new Location(
						(minLat + maxLat) / 2,
						(minLon + maxLon) / 2),
					Math.Max(0.01, maxLat - minLat),
					Math.Max(0.01, maxLon - minLon)
				);

				MyMap.MoveToRegion(bounds);
			}
		});        
    }
}