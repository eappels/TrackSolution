using TrackApp.Helpers;
using TrackApp.ViewModels;

namespace TrackApp.Views;

public partial class TestView : ContentPage
{

	private readonly TestViewModel viewModel;

    public TestView()
	{
		InitializeComponent();

        BindingContext = viewModel = ServiceHelper.GetService<TestViewModel>();

        MyMap.MapElements.Add(viewModel.Track);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.LoadData();
    }
}