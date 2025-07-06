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
        var viewModel = BindingContext as ViewModels.HistoryViewModel;
        viewModel.LoadHistoryView();
        if (MyMap != null)
        {
            MyMap.MapElements.Add(viewModel.Track);
        }
    }
}