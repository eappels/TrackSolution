using TrackApp.ViewModels;

namespace TrackApp.Views;

public partial class HistoryView : ContentPage
{

    public HistoryView()
	{
		InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((HistoryViewModel)BindingContext).LoadHistoryView();
        if (MyMap != null)
        {
            MyMap.MapElements.Add(((HistoryViewModel)BindingContext).Track);
        }
    }
}