using TrackApp.Helpers;
using TrackApp.ViewModels;

namespace TrackApp.Views;

public partial class SettingsView : ContentPage
{

	private readonly SettingsViewModel viewModel;

    public SettingsView()
	{
		InitializeComponent();

        BindingContext = viewModel = ServiceHelper.GetService<SettingsViewModel>();
    }
}