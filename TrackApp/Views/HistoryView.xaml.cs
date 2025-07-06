using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Diagnostics;

namespace TrackApp.Views;

[QueryProperty(nameof(Track), "Track")]
public partial class HistoryView : ContentPage
{

    public HistoryView()
	{
		InitializeComponent();

        if (MyMap != null)
            MyMap.MapElements.Add(Track);

        MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(51.16614100358892, 3.8475931196269197), Distance.FromMeters(150)));



        //Track.Geopath.Add(new Location(51.166140825384396, 3.847582380498502));
        //Track.Geopath.Add(new Location(51.166549387101995, 3.847645878989395));
        //Track.Geopath.Add(new Location(51.16693370879934, 3.8476237925597685));
    }

    private Polyline track = new();
    public Polyline Track
    {
        get => track;
        set
        {
            track = value;
            OnPropertyChanged();
        }
    }
}